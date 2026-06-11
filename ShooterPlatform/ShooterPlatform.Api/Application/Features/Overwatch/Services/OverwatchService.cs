using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Overwatch.Services
{
    public class OverwatchService : IOverwatchService
    {
        private readonly IOverwatchProfileProvider _provider;

        public async Task<OverwatchProfileResponse> GetProfileAsync(string battleTag)
        {
            return await _provider.FetchAsync(battleTag);
        }
    }
}
