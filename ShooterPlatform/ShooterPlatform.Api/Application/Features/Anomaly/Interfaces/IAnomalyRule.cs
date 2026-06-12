using ShooterPlatform.Api.Application.Features.Anomaly.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Interfaces
{
    public interface IAnomalyRule
    {
        Task<AnomalyFlag?> EvaluateAsync(OverwatchProfileResponse profile);
    }
}
