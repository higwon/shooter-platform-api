using ShooterPlatform.Api.Application.Features.Players.DTOs;
using ShooterPlatform.Api.Domain.Entities;

namespace ShooterPlatform.Api.Application.Features.Players.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerResponse ToResponse(Player entity)
        {
            return new PlayerResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Level = entity.Level
            };
        }

        public static Player ToEntity(PlayerCreateRequest request)
        {
            return new Player
            {
                Name = request.Name,
                Level = request.Level
            };
        }

        public static void UpdateEntity(Player entity, PlayerUpdateRequest request)
        {
            entity.Name = request.Name;
            entity.Level = request.Level;
        }
    }
}
