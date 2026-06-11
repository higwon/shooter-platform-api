using ShooterPlatform.Api.Application.Features.Favorite.Interfaces;
using ShooterPlatform.Api.Application.Features.Overwatch.Interfaces;
using ShooterPlatform.Api.Domain.Entities;
using ShooterPlatform.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ShooterPlatform.Api.Application.Features.Favorite.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly ShooterPlatformDbContext _dbContext;
        private readonly IOverwatchService _overwatchService;

        public FavoriteService(
            ShooterPlatformDbContext dbContext,
            IOverwatchService overwatchService)
        {
            _dbContext = dbContext;
            _overwatchService = overwatchService;
        }

        public async Task<FavoritePlayer> AddFavoriteAsync(int userId, string battleTag)
        {
            var exists = await _dbContext.FavoritePlayers
                .AnyAsync(x => x.UserId == userId &&
                               x.BattleTag == battleTag);

            if (exists)
                throw new InvalidOperationException("Already registered.");

            var profile = await _overwatchService.GetProfileAsync(battleTag);

            var favoritePlayer = new FavoritePlayer
            {
                UserId = userId,
                BattleTag = battleTag,
                CachedUsername = profile.Summary.Username,
                CachedAvatar = profile.Summary.Avatar
            };

            _dbContext.FavoritePlayers.Add(favoritePlayer);

            await _dbContext.SaveChangesAsync();

            return favoritePlayer;
        }

        public async Task<List<FavoritePlayer>> GetFavoritesAsync(int userId)
        {
            return await _dbContext.FavoritePlayers
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task DeleteFavoriteAsync(int userId, int favoriteId)
        {
            var favorite = await _dbContext.FavoritePlayers
                .FirstOrDefaultAsync(x =>
                    x.Id == favoriteId &&
                    x.UserId == userId);

            if (favorite == null)
                throw new KeyNotFoundException("Favorite not found.");

            _dbContext.FavoritePlayers.Remove(favorite);

            await _dbContext.SaveChangesAsync();
        }
    }
}
