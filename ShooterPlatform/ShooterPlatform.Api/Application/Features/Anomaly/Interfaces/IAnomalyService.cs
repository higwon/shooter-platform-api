using ShooterPlatform.Api.Application.Features.Anomaly.Models;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Interfaces
{
    public interface IAnomalyService
    {
        Task<AnomalyResult> AnalyzeAsync(string battleTag);
    }
}
