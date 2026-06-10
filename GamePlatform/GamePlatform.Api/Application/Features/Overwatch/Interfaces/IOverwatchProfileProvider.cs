using GamePlatform.Api.Application.Features.Overwatch.DTOs;

namespace GamePlatform.Api.Application.Features.Overwatch.Interfaces
{
    public interface IOverwatchProfileProvider
    {
        Task<OverwatchProfileResponse> FetchAsync(string battleTag);
    }
}
