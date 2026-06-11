using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Overwatch.Interfaces
{
    public interface IOverwatchProfileProvider
    {
        Task<OverwatchProfileResponse> FetchAsync(string battleTag);
    }
}
