using GamePlatform.Api.Models;

namespace GamePlatform.Api.Services;

public interface IPlayerService
{
    IEnumerable<Player> GetPlayers();

    Player? GetPlayer(int id);

    void AddPlayer(Player player);

    Player? UpdatePlayer(int id, Player player);

    bool DeletePlayer(int id);
}