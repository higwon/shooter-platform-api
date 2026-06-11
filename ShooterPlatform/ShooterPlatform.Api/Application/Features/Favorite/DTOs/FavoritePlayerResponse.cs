namespace ShooterPlatform.Api.Application.Features.Favorite.DTOs
{
    public class FavoritePlayerResponse
    {
        public int Id { get; set; }

        public string BattleTag { get; set; }

        public string? Username { get; set; }

        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
