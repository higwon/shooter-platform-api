using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Models;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IAnalysisResultService
    {
        Task<AnalysisResult?> GetByBattleTagAsync(
            string battleTag);

        Task<List<AnalysisResult>> GetByBattleTagsAsync(
            IEnumerable<string> battleTags);

        Task SaveOrUpdateAsync(
            string battleTag,
            ProfileAnalysisResult result);
    }
}
