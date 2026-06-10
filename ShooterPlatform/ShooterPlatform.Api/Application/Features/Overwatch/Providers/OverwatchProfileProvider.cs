using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;
using System.Text.Json;

namespace ShooterPlatform.Api.Application.Features.Overwatch.Providers
{
    public class OverwatchProfileProvider : IOverwatchProfileProvider
    {
        private readonly HttpClient _httpClient;

        public OverwatchProfileProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OverwatchProfileResponse> FetchAsync(string battleTag)
        {
            var url = $"https://overfast-api.tekrop.fr/players/{battleTag}";

            var json = await _httpClient.GetStringAsync(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<OverwatchProfileResponse>(json, options)!;
        }

    }
}
