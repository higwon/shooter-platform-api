using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Analysis.Services
{
    public class ProfileAnalysisService : IProfileAnalysisService
    {
        private readonly IEnumerable<IProfileAnalysisRule> _rules;

        public ProfileAnalysisService(IEnumerable<IProfileAnalysisRule> rules)
        {
            _rules = rules;
        }

        public async Task<ProfileAnalysisResult> AnalyzeAsync(OverwatchProfileResponse profile)
        {
            var flags = new List<ProfileAnalysisFlag>();

            foreach (var rule in _rules)
            {
                var flag = await rule.EvaluateAsync(profile);

                if (flag != null)
                {
                    flags.Add(flag);
                }
            }

            var riskScore = flags.Sum(x => x.Score);

            return new ProfileAnalysisResult
            {
                RiskScore = riskScore,
                RiskLevel = CalculateRiskLevel(riskScore),
                Flags = flags
            };
        }

        private static RiskLevel CalculateRiskLevel(int riskScore)
        {
            if (riskScore >= 60)
                return RiskLevel.High;

            if (riskScore >= 30)
                return RiskLevel.Medium;

            return RiskLevel.Low;
        }
    }
}
