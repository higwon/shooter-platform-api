using ShooterPlatform.Api.Application.Features.Anomaly.Interfaces;
using ShooterPlatform.Api.Application.Features.Anomaly.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;

namespace ShooterPlatform.Api.Application.Features.Anomaly.Services
{
    public class AnomalyService : IAnomalyService
    {
        private readonly IOverwatchService _overwatchService;

        private readonly IEnumerable<IAnomalyRule> _rules;

        public AnomalyService(IOverwatchService overwatchService, IEnumerable<IAnomalyRule> rules)
        {
            _overwatchService = overwatchService;
            _rules = rules;
        }

        public async Task<AnomalyResult> AnalyzeAsync(string battleTag)
        {
            var profile = await _overwatchService.GetProfileAsync(battleTag);

            var flags = new List<AnomalyFlag>();

            foreach (var rule in _rules)
            {
                var flag = await rule.EvaluateAsync(profile);

                if (flag != null)
                {
                    flags.Add(flag);
                }
            }

            var riskScore = flags.Sum(x => x.Score);

            return new AnomalyResult
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
