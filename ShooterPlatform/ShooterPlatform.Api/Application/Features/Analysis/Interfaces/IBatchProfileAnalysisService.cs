using ShooterPlatform.Api.Application.Features.Analysis.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IBatchProfileAnalysisService
    {
        Task<List<ProfileAnalysisResponse>> AnalyzeFavoritesAsync(int userId);
    }
}
