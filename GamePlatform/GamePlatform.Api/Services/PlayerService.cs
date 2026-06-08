using GamePlatform.Api.Models;

namespace GamePlatform.Api.Services;

public class PlayerService : IPlayerService
{
    public IEnumerable<Player> GetPlayers()
    {
        return
        [
            new Player
            {
                Id = 1,
                Name = "HyuckJin",
                Level = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Player
            {
                Id = 2,
                Name = "Player2",
                Level = 50,
                CreatedAt = DateTime.UtcNow
            }
        ];
    }

    public Player? GetPlayer(int id)
    {
        return GetPlayers().FirstOrDefault(x => x.Id == id);
    }
}