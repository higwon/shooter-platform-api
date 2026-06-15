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

            var context = AnalysisTestHelper.CreateContext(profile);

            var result = await _rule.EvaluateAsync(context);

            Assert.NotNull(profile);

            if (result != null)
            {
                Assert.NotEmpty(result.Code);
                Assert.NotEmpty(result.Message);
                Assert.True(result.Score > 0);
            }
        }

        [Fact]
        public async Task EvaluateAsync_ShouldReturnFlag_WhenAccuracyIsHigh()
        {
            var rule = new AccuracyRule();

            var context = new ProfileAnalysisContext
            {
                HeroComparisons = new HeroComparisons
                {
                    WeaponAccuracyBestInGame = new HeroMetric
                    {
                        Values =
                        [
                            new HeroMetricValue
                            {
                                Hero = "Widowmaker",
                                Value = 85
                            }
                        ]
                    },

                    GamesWon = new HeroMetric
                    {
                        Values =
                        [
                            new HeroMetricValue
                            {
                                Hero = "Widowmaker",
                                Value = 50
                            }
                        ]
                    },

                    TimePlayed = new HeroMetric
                    {
                        Values =
                        [
                            new HeroMetricValue
                            {
                                Hero = "Widowmaker",
                                Value = 3000
                            }
                        ]
                    }
                }
            };

            var result = await rule.EvaluateAsync(context);

            Assert.NotNull(result);
            Assert.Equal("HIGH_ACCURACY", result.Code);
        }
    }
}