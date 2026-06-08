using GamePlatform.Api.Data;
using GamePlatform.Api.Models;

namespace GamePlatform.Api.Services;

public class PlayerService : IPlayerService
{
    private readonly GamePlatformDbContext _dbContext;

    public PlayerService(GamePlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Player> GetPlayers()
    {
        return _dbContext.Players.ToList();
    }

    public Player? GetPlayer(int id)
    {
        return _dbContext.Players.FirstOrDefault(x => x.Id == id);
    }
}