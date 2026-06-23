using ShooterPlatform.Api.Application.Features.Analysis.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IAnalysisJobService
    {
        Task RefreshAllAsync();
    }
}
