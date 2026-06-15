using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Rules
{
    public class WinRateRule : IProfileAnalysisRule
    {
        private const double MinWinRate = 80;
        private const double MinGamesWon = 10;
        private const double MinTimePlayed = 600;

        public Task<ProfileAnalysisFlag?> EvaluateAsync(
            ProfileAnalysisContext context)
        {
            var comparisons = context.HeroComparisons;

            if (comparisons == null)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var winRates = comparisons.WinPercentage?.Values;
            var gamesWon = comparisons.GamesWon?.Values;
            var timePlayed = comparisons.TimePlayed?.Values;

            if (winRates == null ||
                gamesWon == null ||
                timePlayed == null)
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var candidates = new List<HeroAnalysis>();

            foreach (var winRateHero in winRates)
            {
                if (string.IsNullOrWhiteSpace(winRateHero.Hero))
                {
                    continue;
                }

                var hero = winRateHero.Hero;

                var wins = gamesWon
                    .FirstOrDefault(x => x.Hero == hero);

                var hours = timePlayed
                    .FirstOrDefault(x => x.Hero == hero);

                if (wins == null || hours == null)
                {
                    continue;
                }

                if (winRateHero.Value < MinWinRate)
                {
                    continue;
                }

                if (wins.Value < MinGamesWon)
                {
                    continue;
                }

                if (hours.Value < MinTimePlayed)
                {
                    continue;
                }

                candidates.Add(new HeroAnalysis
                {
                    Hero = hero,
                    WinRate = winRateHero.Value,
                    GamesWon = wins.Value,
                    TimePlayed = hours.Value
                });
            }

            if (!candidates.Any())
            {
                return Task.FromResult<ProfileAnalysisFlag?>(null);
            }

            var bestHero = candidates
                .OrderByDescending(x => x.WinRate)
                .First();

            var score = bestHero.WinRate >= 90
                ? 40
                : 20;

            return Task.FromResult<ProfileAnalysisFlag?>(
                new ProfileAnalysisFlag
                {
                    Code = "HIGH_WIN_RATE",
                    Message =
                        $"{bestHero.Hero} maintains a {bestHero.WinRate:F1}% win rate " +
                        $"with {bestHero.GamesWon:F0} wins and {bestHero.TimePlayed:F1} hours played.",
                    Score = score
                });
        }

        private class HeroAnalysis
        {
            public string Hero { get; set; } = string.Empty;

            public double WinRate { get; set; }

            public double GamesWon { get; set; }

            public double TimePlayed { get; set; }
        }
    }
}