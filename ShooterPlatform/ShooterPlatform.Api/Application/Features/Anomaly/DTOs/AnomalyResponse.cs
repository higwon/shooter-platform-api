using ShooterPlatform.Api.Application.Features.Anomaly.Models;
using ShooterPlatform.Api.Application.Features.Overwatch.DTOs;

namespace ShooterPlatform.Api.Application.Features.Anomaly.DTOs
{
    public class AnomalyResponse
    {
        public OverwatchProfileResponse Profile { get; set; }
        public AnomalyResult Result { get; set; }
    }
}
