using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(GameStoreContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (!await dbContext.Genres.AnyAsync())
        {
            dbContext.Genres.AddRange(
                new Genre { Name = "Action" },
                new Genre { Name = "RPG" },
                new Genre { Name = "Adventure" });

            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Games.AnyAsync())
        {
            var action = await dbContext.Genres.SingleAsync(genre => genre.Name == "Action");
            var rpg = await dbContext.Genres.SingleAsync(genre => genre.Name == "RPG");

            dbContext.Games.AddRange(
                new Game
                {
                    Name = "Game 1",
                    GenreId = action.Id,
                    Price = 59.99m,
                    ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
                },
                new Game
                {
                    Name = "Game 2",
                    GenreId = rpg.Id,
                    Price = 39.99m,
                    ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
                });

            await dbContext.SaveChangesAsync();
        }
    }
}
