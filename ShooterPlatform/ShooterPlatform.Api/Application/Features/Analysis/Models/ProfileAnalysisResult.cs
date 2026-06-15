namespace ShooterPlatform.Api.Application.Features.Analysis.Models
{
    public class ProfileAnalysisResult
    {
        public int RiskScore { get; set; }

        public RiskLevel RiskLevel { get; set; }

        public List<ProfileAnalysisFlag> Flags { get; set; } = [];
    }
}
