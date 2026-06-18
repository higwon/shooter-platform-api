using ShooterPlatform.Api.Application.Features.Analysis.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Analysis.DTOs
{
    public class ProfileAnalysisResponse
    {
        public ProfileSummary ProfileSummary { get; set; }
        public ProfileAnalysisResult Result { get; set; }
    }
}
