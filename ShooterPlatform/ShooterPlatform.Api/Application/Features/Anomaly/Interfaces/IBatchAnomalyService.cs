using ShooterPlatform.Api.Application.Features.Anomaly.Models;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Interfaces
{
    public interface IBatchAnomalyService
    {
        Task<List<AnomalyResult>> AnalyzeFavoritesAsync(int userId);
    }
}
