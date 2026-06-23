using ShooterPlatform.Api.Application.Features.Analysis.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.Models
{
    public class AnalysisResult
    {
        public int Id { get; set; }

        public string BattleTag { get; set; } = string.Empty;

        public int RiskScore { get; set; }

        public RiskLevel RiskLevel { get; set; }

        public string FlagsJson { get; set; } = string.Empty;

        public DateTime AnalyzedAt { get; set; }
    }
}
