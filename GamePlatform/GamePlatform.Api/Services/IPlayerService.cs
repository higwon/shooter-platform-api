using GamePlatform.Api.Models;

namespace GamePlatform.Api.Services;

public interface IPlayerService
{
    IEnumerable<Player> GetPlayers();
}