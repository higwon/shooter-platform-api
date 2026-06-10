using ShooterPlatform.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
