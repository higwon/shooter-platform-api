using GamePlatform.Api.Application.DTOs;
using GamePlatform.Api.Domain.Entities;

namespace GamePlatform.Api.Application.Interfaces;

public interface IPlayerService
{
    IEnumerable<Player> GetPlayers();

    Player? GetPlayer(int id);

    void AddPlayer(PlayerDto dto);

    Player? UpdatePlayer(int id, Player player);

    bool DeletePlayer(int id);
}