using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;

namespace ShooterPlatform.Api.Application.Features.Analysis.Rules
{
    public class OneTrickRule : IProfileAnalysisRule
    {
        private const double MinPlayRate = 70;

        public Task<ProfileAnalysisFlag?> EvaluateAsync(
            ProfileAnalysisContext context)
        {
            var timePlayed =
                context.HeroComparisons.TimePlayed?.Values;

            if (timePlayed == null || !timePlayed.Any())
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var totalPlayTime =
                timePlayed.Sum(x => x.Value);

            if (totalPlayTime <= 0)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var mostPlayedHero =
                timePlayed.OrderByDescending(x => x.Value)
                          .First();

            var playRate =
                (mostPlayedHero.Value / totalPlayTime) * 100;

            if (playRate < MinPlayRate)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var score = playRate switch
            {
                >= 90 => 40,
                >= 80 => 30,
                _ => 20
            };

            return Task.FromResult<ProfileAnalysisFlag?>(
                new ProfileAnalysisFlag
                {
                    Code = "ONE_TRICK_PLAYER",
                    Message =
                        $"{mostPlayedHero.Hero} accounts for {playRate:F1}% of total playtime.",
                    Score = score
                });
        }
    }
}