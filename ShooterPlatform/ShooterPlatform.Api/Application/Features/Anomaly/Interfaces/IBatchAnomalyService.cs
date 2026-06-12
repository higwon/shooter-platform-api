using ShooterPlatform.Api.Application.Features.Anomaly.DTOs;
using ShooterPlatform.Api.Application.Features.Anomaly.Models;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Interfaces
{
    public interface IBatchAnomalyService
    {
        Task<List<AnomalyResponse>> AnalyzeFavoritesAsync(int userId);
    }
}
