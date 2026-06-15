using System.Text.Json.Serialization;

namespace ShooterPlatform.Api.Application.Features.Overwatch.DTOs
{
    public class OverwatchProfileResponse
    {
        public ProfileSummary Summary { get; set; }

        public ProfileStats? Stats { get; set; }
    }

    public class ProfileSummary
    {
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Namecard { get; set; }
        public string? title { get; set; }
        public Endorsement Endorsement { get; set; }
        public Competitive Competitive { get; set; }
    }

    public class Endorsement
    {
        public int Level { get; set; }
        public string Frame { get; set; }
    }

    public class Competitive
    {
        public PlatformCompetitive? Pc { get; set; }
        public PlatformCompetitive? Console { get; set; }
    }

    public class PlatformCompetitive
    {
        public int Season { get; set; }

        public Role? Tank { get; set; }
        public Role? Damage { get; set; }
        public Role? Support { get; set; }
    }

    public class Role
    {
        public string? Division { get; set; }
        public int? Tier { get; set; }
        public int? Rank { get; set; }
    }

    public class ProfileStats
    {
        public PlatformStats? Pc { get; set; }

        public PlatformStats? Console { get; set; }
    }

    public class PlatformStats
    {
        public GameModeStats? Quickplay { get; set; }

        public GameModeStats? Competitive { get; set; }
    }

    public class GameModeStats
    {
        [JsonPropertyName("heroes_comparisons")]
        public HeroComparisons? HeroesComparisons { get; set; }
    }

    public class HeroComparisons
    {
        [JsonPropertyName("time_played")]
        public HeroMetric? TimePlayed { get; set; }

        [JsonPropertyName("games_won")]
        public HeroMetric? GamesWon { get; set; }

        [JsonPropertyName("win_percentage")]
        public HeroMetric? WinPercentage { get; set; }

        [JsonPropertyName("weapon_accuracy_best_in_game")]
        public HeroMetric? WeaponAccuracyBestInGame { get; set; }

        [JsonPropertyName("eliminations_per_life")]
        public HeroMetric? EliminationsPerLife { get; set; }
    }

    public class HeroMetric
    {
        public string? Label { get; set; }

        public List<HeroMetricValue>? Values { get; set; }
    }

    public class HeroMetricValue
    {
        public string? Hero { get; set; }

        public double Value { get; set; }
    }
}
