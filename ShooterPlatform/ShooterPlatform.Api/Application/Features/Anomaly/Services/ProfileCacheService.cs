using Microsoft.Extensions.Caching.Memory;
using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Services
{
    public class ProfileCacheService : IProfileCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IOverwatchService _overwatchService;

        private const int CacheMinutes = 10;

        public ProfileCacheService(
            IMemoryCache cache,
            IOverwatchService overwatchService)
        {
            _cache = cache;
            _overwatchService = overwatchService;
        }

        public async Task<OverwatchProfileResponse> GetOrFetchAsync(string battleTag)
        {
            var key = GetKey(battleTag);

            if (_cache.TryGetValue(key, out OverwatchProfileResponse cached))
                return cached;

            var profile = await _overwatchService.GetProfileAsync(battleTag);

            _cache.Set(key, profile, TimeSpan.FromMinutes(CacheMinutes));

            return profile;
        }

        private static string GetKey(string battleTag)
            => $"overwatch:profile:{battleTag}";
    }
}
