using ShooterPlatform.Api.Application.Features.Favorite.DTOs;
using ShooterPlatform.Api.Domain.Entities;

namespace ShooterPlatform.Api.Application.Features.Favorite.Interfaces
{
    public interface IFavoriteService
    {
        Task<FavoritePlayer> AddFavoriteAsync(int userId, string battleTag);

        Task<List<FavoriteResponse>> GetFavoritesAsync(int userId);

        Task DeleteFavoriteAsync(int userId, int favoriteId);
    }
}
