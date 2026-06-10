using ShooterPlatform.Api.Domain.Enums;

namespace ShooterPlatform.Api.Domain.Entities
{
    public class FavoritePlayer
    {
        public int Id { get; set; }

        public int UserId { get; set; } = default!;

        public User User { get; set; } = default!;

        public string BattleTag { get; set; } = string.Empty;

        public string? CachedUsername { get; set; }

        public string? CachedAvatar { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
