using GamePlatform.Api.Application.Common.CustomExceptions;
using GamePlatform.Api.Application.DTOs;
using GamePlatform.Api.Application.Interfaces;
using GamePlatform.Api.Application.Mappers;
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


        public IEnumerable<PlayerResponse> GetAllPlayers()
        {
            return _dbContext.Players
                .Select(PlayerMapper.ToResponse)
                .ToList();
        }

        public PlayerResponse GetPlayer(int id)
        {
            var player = _dbContext.Players.FirstOrDefault(x => x.Id == id)
                ?? throw new NotFoundException("Player not found");

            return PlayerMapper.ToResponse(player);
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
                .FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException($"Player {id} not found");
            player.Name = request.Name;
            player.Level = request.Level;

            _dbContext.SaveChanges();

            return PlayerMapper.ToResponse(player);
        }

        public void DeletePlayer(int id)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException($"Player {id} not found");

            _dbContext.Players.Remove(player);
            _dbContext.SaveChanges();
        }
    }
}

