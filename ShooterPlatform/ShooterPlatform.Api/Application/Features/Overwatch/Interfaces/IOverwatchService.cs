using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Overwatch.Interfaces
{
    public interface IOverwatchService
    {
        Task<OverwatchProfileResponse> GetProfileAsync(string battleTag);
    }
}
