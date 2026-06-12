using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
using ShooterPlatform.Api.Application.Features.Anomaly.Models;
using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Services
{
    public class BatchAnomalyService : IBatchAnomalyService
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IAnomalyService _anomalyService;
        private readonly IProfileCacheService _profileCacheService;

        public BatchAnomalyService(
            IFavoriteService favoriteService,
            IAnomalyService anomalyService,
            IProfileCacheService profileCacheService)
        {
            _favoriteService = favoriteService;
            _anomalyService = anomalyService;
            _profileCacheService = profileCacheService;
        }

        public async Task<List<AnomalyResult>> AnalyzeFavoritesAsync(int userId)
        {
            var favorites = await _favoriteService.GetFavoritesAsync(userId);

            var tasks = favorites.Select(async f =>
            {
                var profile = await _profileCacheService
                    .GetOrFetchAsync(f.BattleTag);

                var result = await _anomalyService
                    .AnalyzeAsync(f.BattleTag);

                return result;
            });

            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }
    }
}
