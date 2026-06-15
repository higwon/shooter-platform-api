using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Contexts
{
    public class ProfileAnalysisContext
    {
        public string Platform { get; set; } = string.Empty;

        public HeroComparisons HeroComparisons { get; set; } = null!;

        public OverwatchProfileResponse Profile { get; set; } = null!;
    }
}
