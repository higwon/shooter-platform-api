using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Analysis.Rules
{
    public class AccuracyRule : IProfileAnalysisRule
    {
        private const double MinAccuracy = 65;
        private const double MinGamesWon = 10;
        private const double MinTimePlayed = 600;

        public Task<ProfileAnalysisFlag?> EvaluateAsync(
            ProfileAnalysisContext context)
        {
            var comparisons = context.HeroComparisons;

            var accuracies = comparisons.WeaponAccuracyBestInGame?.Values;
            var gamesWon = comparisons.GamesWon?.Values;
            var timePlayed = comparisons.TimePlayed?.Values;

            if (accuracies == null ||
                gamesWon == null ||
                timePlayed == null)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var candidates = new List<HeroAccuracy>();

            foreach (var accuracyHero in accuracies)
            {
                if (string.IsNullOrWhiteSpace(accuracyHero.Hero))
                {
                    continue;
                }

                var hero = accuracyHero.Hero;

                var wins = gamesWon.FirstOrDefault(x => x.Hero == hero);
                var playtime = timePlayed.FirstOrDefault(x => x.Hero == hero);

                if (wins == null || playtime == null)
                {
                    continue;
                }

                if (accuracyHero.Value < MinAccuracy)
                {
                    continue;
                }

                if (wins.Value < MinGamesWon)
                {
                    continue;
                }

                if (playtime.Value < MinTimePlayed)
                {
                    continue;
                }

                candidates.Add(new HeroAccuracy
                {
                    Hero = hero,
                    Accuracy = accuracyHero.Value,
                    GamesWon = wins.Value,
                    TimePlayed = playtime.Value
                });
            }

            if (!candidates.Any())
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var bestHero = candidates
                .OrderByDescending(x => x.Accuracy)
                .First();

            var score = bestHero.Accuracy switch
            {
                >= 80 => 40,
                >= 70 => 30,
                _ => 20
            };

            return Task.FromResult<ProfileAnalysisFlag?>(
                new ProfileAnalysisFlag
                {
                    Code = "HIGH_ACCURACY",
                    Message =
                        $"{bestHero.Hero} has {bestHero.Accuracy:F1}% weapon accuracy.",
                    Score = score
                });
        }

        private class HeroAccuracy
        {
            public string Hero { get; set; } = string.Empty;

            public double Accuracy { get; set; }

            public double GamesWon { get; set; }

            public double TimePlayed { get; set; }
        }
    }
}