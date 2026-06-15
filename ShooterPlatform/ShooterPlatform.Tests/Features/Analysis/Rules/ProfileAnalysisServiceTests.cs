using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Rules;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Providers;

namespace ShooterPlatform.Tests.Features.Analysis.Rules
{
    public class WinRateRuleTests
    {
        private readonly OverwatchProfileProvider _provider;
        private readonly WinRateRule _rule;

        public WinRateRuleTests()
        {
            _provider = new OverwatchProfileProvider(new HttpClient());
            _rule = new WinRateRule();
        }

        [Fact]
        public async Task EvaluateAsync_ShouldAnalyzeRealProfile()
        {
            var battleTag =
                "c6718fa29a3cc9fcbfa025%7C3f7926bddbe82d5c9e0f2e7d181b2d2e";

            var profile = await _provider.FetchAsync(battleTag);

            var comparisons =
                profile.Stats?.Pc?.Competitive?.HeroesComparisons
                ?? profile.Stats?.Console?.Competitive?.HeroesComparisons;

            Assert.NotNull(comparisons);

            var context = new ProfileAnalysisContext
            {
                Platform = profile.Stats?.Pc?.Competitive != null
                    ? "PC"
                    : "Console",

                HeroComparisons = comparisons!,
                Profile = profile
            };

            var result = await _rule.EvaluateAsync(context);

            Assert.NotNull(profile);

            if (result != null)
            {
                Assert.NotEmpty(result.Code);
                Assert.NotEmpty(result.Message);
                Assert.True(result.Score > 0);
            }
        }
    }
}