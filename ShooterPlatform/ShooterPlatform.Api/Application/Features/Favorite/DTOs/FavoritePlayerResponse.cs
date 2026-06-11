namespace ShooterPlatform.Api.Application.Features.Favorite.DTOs
{
    public class FavoriteResponse
    {
        public int Id { get; set; }

        public string BattleTag { get; set; }

        public string Username { get; set; }

        public string Avatar { get; set; }

        public string? TankRank { get; set; }

        public string? DamageRank { get; set; }

        public string? SupportRank { get; set; }
    }
}
