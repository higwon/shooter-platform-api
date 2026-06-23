using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Interfaces;
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
            var context = BuildContext(profile);

            if (context == null)
            {
                return new ProfileAnalysisResult
                {
                    RiskLevel = RiskLevel.Low,
                    RiskScore = 0,
                    Flags = []
                };
            }


            var flags = new List<ProfileAnalysisFlag>();

            foreach (var rule in _rules)
            {
                var flag = await rule.EvaluateAsync(context);

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

        private static ProfileAnalysisContext? BuildContext(OverwatchProfileResponse profile)
        {
            var pcComparisons =
                profile.Stats?
                       .Pc?
                       .Competitive?
                       .HeroesComparisons;

            if (pcComparisons != null)
            {
                return new ProfileAnalysisContext
                {
                    Platform = "PC",
                    HeroComparisons = pcComparisons,
                    Profile = profile
                };
            }

            var consoleComparisons =
                profile.Stats?
                       .Console?
                       .Competitive?
                       .HeroesComparisons;

            if (consoleComparisons != null)
            {
                return new ProfileAnalysisContext
                {
                    Platform = "Console",
                    HeroComparisons = consoleComparisons,
                    Profile = profile
                };
            }

            return null;
        }
    }
}
