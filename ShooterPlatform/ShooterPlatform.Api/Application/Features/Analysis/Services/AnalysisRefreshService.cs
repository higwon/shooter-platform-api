using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Analysis.Services
{
    public class AnalysisRefreshService : IAnalysisRefreshService
    {
        private readonly IProfileAnalysisService _profileAnalysisService;
        private readonly IProfileCacheService _profileCacheService;
        private readonly IAnalysisResultService _analysisResultService;

        public AnalysisRefreshService(
            IProfileAnalysisService profileAnalysisService,
            IProfileCacheService profileCacheService,
            IAnalysisResultService analysisResultService)
        {
            _profileAnalysisService = profileAnalysisService;
            _profileCacheService = profileCacheService;
            _analysisResultService = analysisResultService;
        }

        public async Task AnalyzeAndSaveAsync(string battleTag)
        {
            var profile = await _profileCacheService.GetOrFetchAsync(battleTag);

            var result = await _profileAnalysisService
                    .AnalyzeAsync(profile);

            await _analysisResultService
                .SaveOrUpdateAsync(
                    battleTag,
                    result);
        }
    }
}
