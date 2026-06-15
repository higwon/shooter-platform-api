using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IProfileAnalysisService
    {
        Task<ProfileAnalysisResult> AnalyzeAsync(OverwatchProfileResponse profile);
    }
}
