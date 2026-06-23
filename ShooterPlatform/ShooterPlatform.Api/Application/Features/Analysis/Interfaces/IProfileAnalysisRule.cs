using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IProfileAnalysisRule
    {
        Task<ProfileAnalysisFlag?> EvaluateAsync(ProfileAnalysisContext context);
    }
}
