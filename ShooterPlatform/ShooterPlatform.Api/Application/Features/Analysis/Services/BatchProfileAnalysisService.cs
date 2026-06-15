using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Analysis.Services
{
    public class BatchProfileAnalysisService : IBatchProfileAnalysisService
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProfileAnalysisService _profileAnalysisService;
        private readonly IProfileCacheService _profileCacheService;

        public BatchProfileAnalysisService(
            IFavoriteService favoriteService,
            IProfileAnalysisService profileAnalysisService,
            IProfileCacheService profileCacheService)
        {
            _favoriteService = favoriteService;
            _profileAnalysisService = profileAnalysisService;
            _profileCacheService = profileCacheService;
        }

        public async Task<List<ProfileAnalysisResponse>> AnalyzeFavoritesAsync(int userId)
        {
            var favorites = await _favoriteService.GetFavoritesAsync(userId);

            var tasks = favorites.Select(async f =>
            {
                var profile = await _profileCacheService
                    .GetOrFetchAsync(f.BattleTag);

                var result = await _profileAnalysisService
                    .AnalyzeAsync(profile);

                return new ProfileAnalysisResponse
                {
                    Profile = profile,
                    Result = result
                };
            });

            var results = await Task.WhenAll(tasks);

            return results.ToList();
        }
    }
}
