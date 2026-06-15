using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Interfaces
{
    public interface IProfileCacheService
    {
        Task<OverwatchProfileResponse> GetOrFetchAsync(string battleTag);
    }
}
