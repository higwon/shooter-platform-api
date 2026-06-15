using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Rules;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Providers;

namespace ShooterPlatform.Tests.Features.Analysis.Rules
{
    public class AccuracyRuleTests
    {
        private readonly OverwatchProfileProvider _provider;
        private readonly AccuracyRule _rule;

        public AccuracyRuleTests()
        {
            _provider = new OverwatchProfileProvider(new HttpClient());
            _rule = new AccuracyRule();
        }

        [Fact]
        public async Task EvaluateAsync_ShouldAnalyzeRealProfile()
        {
            var battleTag =
                "c6718fa29a3cc9fcbfa025%7C3f7926bddbe82d5c9e0f2e7d181b2d2e";

            var profile = await _provider.FetchAsync(battleTag);

            var context = AnalysisTestHelper.CreateContext(profile);

            var result = await _rule.EvaluateAsync(context);

            Assert.NotNull(profile);

            if (result != null)
            {
                Assert.Equal("HIGH_ACCURACY", result.Code);
                Assert.NotEmpty(result.Message);
                Assert.True(result.Score > 0);
            }
        }

        [Fact]
        public async Task EvaluateAsync_ShouldReturnFlag_WhenWinRateIsHigh()
        {
            var rule = new WinRateRule();

            var context = new ProfileAnalysisContext
            {
                HeroComparisons = new HeroComparisons
                {
                    WinPercentage = new HeroMetric
                    {
                        Values =
                        [
                            new HeroMetricValue
                            {
                                Hero = "Widowmaker",
                                Value = 90
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
            Assert.Equal("HIGH_WIN_RATE", result.Code);
        }
    }
}