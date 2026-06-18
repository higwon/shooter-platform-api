using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Analysis.Services
{
    public class ProfileCacheService : IProfileCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly IOverwatchService _overwatchService;
        private readonly ILogger<ProfileCacheService> _logger;

        private static readonly TimeSpan MemoryCacheDuration = TimeSpan.FromMinutes(5);

        private static readonly TimeSpan RedisCacheDuration = TimeSpan.FromMinutes(10);

        public ProfileCacheService(
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IOverwatchService overwatchService,
            ILogger<ProfileCacheService> logger)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _overwatchService = overwatchService;
            _logger = logger;
        }

        public async Task<OverwatchProfileResponse> GetOrFetchAsync(string battleTag)
        {
            var key = GetKey(battleTag);

            // L1 Cache (Memory)
            if (_memoryCache.TryGetValue(key, out OverwatchProfileResponse? memoryProfile))
            {
                return memoryProfile!;
            }

            // L2 Cache (Redis)
            try
            {
                var cachedJson = await _distributedCache.GetStringAsync(key);

                if (!string.IsNullOrWhiteSpace(cachedJson))
                {
                    var cachedProfile =JsonSerializer.Deserialize<OverwatchProfileResponse>(cachedJson);

                    if (cachedProfile is not null)
                    {
                        _memoryCache.Set(
                            key,
                            cachedProfile,
                            MemoryCacheDuration);

                        return cachedProfile;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to get profile from Redis. Key: {CacheKey}",
                    key);
            }

            var profile =
                await _overwatchService.GetProfileAsync(battleTag);

            try
            {
                await _distributedCache.SetStringAsync(
                    key,
                    JsonSerializer.Serialize(profile),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow =
                            RedisCacheDuration
                    });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to save profile to Redis. Key: {CacheKey}",
                    key);
            }

            _memoryCache.Set(
                key,
                profile,
                MemoryCacheDuration);

            return profile;
        }

        private static string GetKey(string battleTag)
        {
            return $"overwatch:profile:{battleTag.ToLowerInvariant()}";
        }
    }
}