namespace ShooterPlatform.Api.Application.Features.Anomaly.Models
{
    public class AnomalyFlag
    {
        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int Score { get; set; }
    }
}
