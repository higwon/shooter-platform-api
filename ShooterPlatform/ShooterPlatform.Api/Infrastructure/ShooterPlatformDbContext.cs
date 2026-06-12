using Microsoft.EntityFrameworkCore;
using ShooterPlatform.Api.Application.Features.Favorite.Models;
using ShooterPlatform.Api.Application.Features.Auth.Models;
using ShooterPlatform.Api.Application.Features.Players.Models;

namespace ShooterPlatform.Api.Infrastructure
{
    public class ShooterPlatformDbContext : DbContext
    {
        public ShooterPlatformDbContext(DbContextOptions<ShooterPlatformDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<FavoritePlayer> FavoritePlayers => Set<FavoritePlayer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FavoritePlayer>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.User)
                      .WithMany(x => x.FavoritePlayers)
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.UserId, x.BattleTag })
                      .IsUnique();

                entity.Property(x => x.BattleTag)
                      .HasMaxLength(100)
                      .IsRequired();
            });
        }

    }
}
