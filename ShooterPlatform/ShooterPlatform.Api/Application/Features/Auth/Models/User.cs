using ShooterPlatform.Api.Application.Features.Favorite.Models;

namespace ShooterPlatform.Api.Application.Features.Auth.Models
{
    public class User
    {
        public int Id { get; set; } = default!;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public List<FavoritePlayer> FavoritePlayers { get; set; } = new();
    }
}
