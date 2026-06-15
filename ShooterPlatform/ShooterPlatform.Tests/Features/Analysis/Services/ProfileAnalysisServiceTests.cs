using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Rules;
using ShooterPlatform.Api.Application.Features.Analysis.Services;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Tests.Features.Analysis.Services
{
    public class ProfileAnalysisServiceTests
    {
        [Fact]
        public async Task AnalyzeAsync_ShouldExecuteAllRules()
        {
            var rules = new List<IProfileAnalysisRule>
            {
                new WinRateRule(),
                new OneTrickRule(),
                new AccuracyRule()
            };

            var service = new ProfileAnalysisService(rules);

            var profile = new OverwatchProfileResponse
            {
                Stats = new ProfileStats
                {
                    Pc = new PlatformStats
                    {
                        Competitive = new GameModeStats
                        {
                            HeroesComparisons = new HeroComparisons
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
                                            Value = 9000
                                        },

                                        new HeroMetricValue
                                        {
                                            Hero = "Tracer",
                                            Value = 1000
                                        }
                                    ]
                                }
                            }
                        }
                    }
                }
            };

            var result = await service.AnalyzeAsync(profile);

            Assert.NotNull(result);

            Assert.Equal(3, result.Flags.Count);

            Assert.True(result.RiskScore > 0);
        }
    }
}