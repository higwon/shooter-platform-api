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
    }
}
