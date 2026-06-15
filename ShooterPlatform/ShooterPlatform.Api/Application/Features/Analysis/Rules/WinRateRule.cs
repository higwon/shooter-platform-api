using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Rules
{
    public class WinRateRule : IProfileAnalysisRule
    {
        public Task<ProfileAnalysisFlag?> EvaluateAsync(OverwatchProfileResponse profile)
        {
            var winRateMetric =
                       profile.Stats?
                              .Console?
                              .Competitive?
                              .HeroesComparisons?
                              .WinPercentage;

            if (winRateMetric?.Values == null ||
                !winRateMetric.Values.Any())
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var highestWinRate =
                winRateMetric.Values.Max(x => x.Value);

            if (highestWinRate < 80)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            return Task.FromResult<ProfileAnalysisFlag?>(
                new ProfileAnalysisFlag
                {
                    Code = "HIGH_WIN_RATE",
                    Message = $"Hero win rate is unusually high ({highestWinRate:F1}%).",
                    Score = highestWinRate >= 90 ? 40 : 20
                });
        }
    }
}
