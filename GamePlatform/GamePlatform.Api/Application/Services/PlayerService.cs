using GamePlatform.Api.Application.Common.CustomExceptions;
using GamePlatform.Api.Application.DTOs;
using GamePlatform.Api.Application.Interfaces;
using GamePlatform.Api.Domain.Entities;
using GamePlatform.Api.Infrastructure;

namespace GamePlatform.Api.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly GamePlatformDbContext _dbContext;

        public PlayerService(GamePlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<PlayerResponse> GetPlayers()
        {
            return _dbContext.Players
                .Select(x => new PlayerResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Level = x.Level
                })
                .ToList();
        }

        public PlayerResponse GetPlayer(int id)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id);

            if (player is null)
                throw new NotFoundException($"Player {id} not found");

            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Level = player.Level
            };
        }

        public void CreatePlayer(PlayerCreateRequest request)
        {
            var player = new Player
            {
                Name = request.Name,
                Level = request.Level
            };

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
        }

        public PlayerResponse UpdatePlayer(int id, PlayerUpdateRequest request)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id);

            if (player is null)
                throw new NotFoundException($"Player {id} not found");

            player.Name = request.Name;
            player.Level = request.Level;

            _dbContext.SaveChanges();

            return new PlayerResponse
            {
                Id = player.Id,
                Name = player.Name,
                Level = player.Level
            };
        }

        public void DeletePlayer(int id)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id);

            if (player is null)
                throw new NotFoundException($"Player {id} not found");

            _dbContext.Players.Remove(player);
            _dbContext.SaveChanges();
        }
    }
}

