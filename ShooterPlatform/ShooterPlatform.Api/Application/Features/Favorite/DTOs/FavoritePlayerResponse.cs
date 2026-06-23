using ShooterPlatform.Api.Application.Features.Analysis.DTOs;
using ShooterPlatform.Api.Application.Features.Analysis.Models;

namespace ShooterPlatform.Api.Application.Features.Favorite.DTOs
{
    public class FavoriteResponse
    {
        public int Id { get; set; }

        public string BattleTag { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public int? RiskScore { get; set; }

        public RiskLevel? RiskLevel { get; set; }

        public DateTime? AnalyzedAt { get; set; }
    }
}