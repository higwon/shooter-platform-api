namespace ShooterPlatform.Api.Application.Features.Anomaly.Models
{
    public class AnomalyResult
    {
        public int RiskScore { get; set; }

        public RiskLevel RiskLevel { get; set; }

        public List<AnomalyFlag> Flags { get; set; } = [];
    }
}
