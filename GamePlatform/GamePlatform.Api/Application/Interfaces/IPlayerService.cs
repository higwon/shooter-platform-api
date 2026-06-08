using GamePlatform.Api.Domain.Entities;

namespace GamePlatform.Api.Application.Interfaces;

public interface IPlayerService
{
    IEnumerable<Player> GetPlayers();

    Player? GetPlayer(int id);

    void CreatePlayer(PlayerCreateRequest request);

    Player? UpdatePlayer(int id, Player player);

    bool DeletePlayer(int id);
}