using GamePlatform.Api.Application.DTOs;
using GamePlatform.Api.Domain.Entities;

namespace GamePlatform.Api.Application.Mappers
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
