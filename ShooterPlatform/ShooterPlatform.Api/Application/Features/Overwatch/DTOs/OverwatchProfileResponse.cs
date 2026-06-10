namespace ShooterPlatform.Api.Application.Features.Overwatch.DTOs
{
    public class OverwatchProfileResponse
    {
        public ProfileSummary Summary { get; set; }
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
}
