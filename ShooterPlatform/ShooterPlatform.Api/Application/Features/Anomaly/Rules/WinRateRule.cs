using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
using ShooterPlatform.Api.Application.Features.Anomaly.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Rules
{
    public class WinRateRule : IAnomalyRule
    {
        public Task<AnomalyFlag?> EvaluateAsync(OverwatchProfileResponse profile)
        {
            return Task.FromResult<AnomalyFlag?>(
                new AnomalyFlag
                {
                    Code = "HIGH_WIN_RATE",
                    Message = "High win rate detected.",
                    Score = 20
                });
        }
    }
}
