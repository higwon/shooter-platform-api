using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Rules
{
    public class WinRateRule : IProfileAnalysisRule
    {
        public Task<ProfileAnalysisFlag?> EvaluateAsync(OverwatchProfileResponse profile)
        {
            return Task.FromResult<ProfileAnalysisFlag?>(
                new ProfileAnalysisFlag
                {
                    Code = "HIGH_WIN_RATE",
                    Message = "High win rate detected.",
                    Score = 20
                });
        }
    }
}
