using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.DTOs
{
    public class ProfileAnalysisResponse
    {
        public OverwatchProfileResponse Profile { get; set; }
        public ProfileAnalysisResult Result { get; set; }
    }
}
