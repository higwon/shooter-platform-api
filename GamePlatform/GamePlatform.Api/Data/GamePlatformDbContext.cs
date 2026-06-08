using GamePlatform.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Api.Data;

public class GamePlatformDbContext : DbContext
{
    public GamePlatformDbContext(DbContextOptions<GamePlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
}