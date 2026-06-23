namespace ShooterPlatform.Api.Application.Features.Analysis.DTOs
{
    public class ProfileAnalysisResult
    {
        public int RiskScore { get; set; }

        public RiskLevel RiskLevel { get; set; }

        public List<ProfileAnalysisFlag> Flags { get; set; } = [];
    }
}
