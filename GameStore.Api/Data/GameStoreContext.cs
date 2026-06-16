using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .Property(game => game.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Game>()
            .Property(game => game.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Genre>()
            .Property(genre => genre.Name)
            .HasMaxLength(50);

        modelBuilder.Entity<Genre>()
            .HasIndex(genre => genre.Name)
            .IsUnique();
    }
}
