using ShooterPlatform.Api.Application.Features.Analysis.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IAnalysisRefreshService
    {
        Task AnalyzeAndSaveAsync(string battleTag);
    }
}
