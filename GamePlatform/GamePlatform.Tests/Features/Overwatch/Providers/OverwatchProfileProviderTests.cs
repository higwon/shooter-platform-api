using GamePlatform.Api.Application.Features.Overwatch.DTOs;
using GamePlatform.Api.Application.Features.Overwatch.Providers;

namespace GamePlatform.Tests.Features.Overwatch.Providers
{
    public class OverwatchProfileProviderTests
    {
        private readonly OverwatchProfileProvider _provider;

        public OverwatchProfileProviderTests()
        {
            var httpClient = new HttpClient();

            _provider = new OverwatchProfileProvider(httpClient);
        }

        [Fact]
        public async Task FetchAsync_ShouldReturn_PlayerProfileRaw()
        {
            // Arrange
            var battleTag = "c6718fa29a3cc9fcbfa025%7C3f7926bddbe82d5c9e0f2e7d181b2d2e";

            // Act
            OverwatchProfileResponse result = await _provider.FetchAsync(battleTag);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Summary.Username));

        }
    }
}