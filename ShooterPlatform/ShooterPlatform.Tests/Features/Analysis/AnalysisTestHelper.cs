using ShooterPlatform.Api.Application.Features.Analysis.Contexts;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShooterPlatform.Tests.Features.Analysis
{
    internal static class AnalysisTestHelper
    {
        public static ProfileAnalysisContext CreateContext(
            OverwatchProfileResponse profile)
        {
            var comparisons =
                profile.Stats?.Pc?.Competitive?.HeroesComparisons
                ?? profile.Stats?.Console?.Competitive?.HeroesComparisons;

            if (comparisons == null)
            {
                throw new InvalidOperationException(
                    "HeroComparisons not found.");
            }

            return new ProfileAnalysisContext
            {
                Platform = profile.Stats?.Pc?.Competitive != null
                    ? "PC"
                    : "Console",

                HeroComparisons = comparisons,
                Profile = profile
            };
        }
    }
}
