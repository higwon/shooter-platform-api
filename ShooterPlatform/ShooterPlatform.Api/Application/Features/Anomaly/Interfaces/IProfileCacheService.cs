using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Interfaces
{
    public interface IProfileCacheService
    {
        Task<OverwatchProfileResponse> GetOrFetchAsync(string battleTag);
    }
}
