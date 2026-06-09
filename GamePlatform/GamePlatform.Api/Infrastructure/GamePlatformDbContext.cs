using GamePlatform.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamePlatform.Api.Infrastructure
{
    public class GamePlatformDbContext : DbContext
    {
        public GamePlatformDbContext(DbContextOptions<GamePlatformDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
