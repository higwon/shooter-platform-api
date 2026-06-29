using Microsoft.EntityFrameworkCore;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Services;
using ShooterPlatform.Api.Application.Features.Favorite.DTOs;
using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;
using ShooterPlatform.Api.Application.Features.Favorite.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Infrastructure;

namespace ShooterPlatform.Api.Application.Features.Favorite.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ShooterPlatformDbContext _dbContext;
        private readonly IProfileCacheService _profileCacheService;
        private readonly IAnalysisRefreshService _analysisRefreshService;
        private readonly IAnalysisResultService _analysisResultService;

        public FavoriteService(
            ShooterPlatformDbContext dbContext,
            IProfileCacheService profileCacheService,
            IAnalysisRefreshService analysisRefreshService,
            IAnalysisResultService analysisResultService)
        {
            _dbContext = dbContext;
            _profileCacheService = profileCacheService;
            _analysisRefreshService = analysisRefreshService;
            _analysisResultService = analysisResultService;
        }

        public async Task<FavoritePlayer> AddFavoriteAsync(int userId, string battleTag)
        {
            var exists = await _dbContext.FavoritePlayers
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.BattleTag == battleTag);

            if (exists)
                throw new InvalidOperationException(
                    "Already registered.");

            var profile = await _profileCacheService.GetOrFetchAsync(battleTag);

            var favoritePlayer = new FavoritePlayer
            {
                UserId = userId,
                BattleTag = battleTag,
                CachedUsername = profile.Summary.Username,
                CachedAvatar = profile.Summary.Avatar
            };

            _dbContext.FavoritePlayers.Add(favoritePlayer);

            await _dbContext.SaveChangesAsync();

            await _analysisRefreshService
                .AnalyzeAndSaveAsync(battleTag);

            return favoritePlayer;
        }

        public async Task RefreshAnalysisAsync(int userId)
        {
            var battleTags = await _dbContext.FavoritePlayers
                .Where(x => x.UserId == userId)
                .Select(x => x.BattleTag)
                .ToListAsync();

            const int maxConcurrency = 5;

            using var semaphore = new SemaphoreSlim(maxConcurrency);

            var tasks = battleTags.Select(async battleTag =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await _analysisRefreshService
                        .AnalyzeAndSaveAsync(battleTag);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        public async Task<List<FavoriteResponse>> GetFavoritesAsync(int userId)
        {
            var favorites = await _dbContext.FavoritePlayers
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var battleTags = favorites
                .Select(x => x.BattleTag)
                .ToList();

            var analyses = await _analysisResultService
                .GetByBattleTagsAsync(battleTags);

            var analysisMap = analyses.ToDictionary(x => x.BattleTag);

            return favorites.Select(favorite =>
            {
                analysisMap.TryGetValue(favorite.BattleTag, out var analysis);

                return new FavoriteResponse
                {
                    Id = favorite.Id,
                    BattleTag = favorite.BattleTag,

                    Username = favorite.CachedUsername,
                    Avatar = favorite.CachedAvatar,

                    RiskScore = analysis?.RiskScore,
                    RiskLevel = analysis?.RiskLevel,
                    AnalyzedAt = analysis?.AnalyzedAt
                };
            }).ToList();
        }

        public async Task DeleteFavoriteAsync(
            int userId,
            int favoriteId)
        {
            var favorite = await _dbContext.FavoritePlayers
                .FirstOrDefaultAsync(x =>
                    x.Id == favoriteId &&
                    x.UserId == userId);

            if (favorite == null)
                throw new KeyNotFoundException(
                    "Favorite not found.");

            _dbContext.FavoritePlayers.Remove(favorite);

            await _dbContext.SaveChangesAsync();
        }

    }
}