using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoint;

public static class GameEndpoint
{
    public static WebApplication MapGameEndpoints(this WebApplication app)
    {
        app.MapGet("/games", async (GameStoreContext dbContext) =>
            await dbContext.Games
                .Include(game => game.Genre)
                .AsNoTracking()
                .Select(game => game.ToDto())
                .ToListAsync())
            .WithName("GetGames");

        app.MapGet("/games/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games
                .Include(game => game.Genre)
                .AsNoTracking()
                .SingleOrDefaultAsync(game => game.Id == id);

            return game is not null ? Results.Ok(game.ToDto()) : Results.NotFound();
        })
        .WithName("GetGame");

        app.MapPost("/games", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            var genre = await GetOrCreateGenreAsync(newGame.Genre, dbContext);

            var game = new Game
            {
                Name = newGame.Name,
                GenreId = genre.Id,
                Genre = genre,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToDto());
        });

        app.MapPut("/games/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            var genre = await GetOrCreateGenreAsync(updatedGame.Genre, dbContext);

            game.Name = updatedGame.Name;
            game.GenreId = genre.Id;
            game.Price = updatedGame.Price;
            game.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/games/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }

    private static async Task<Genre> GetOrCreateGenreAsync(string genreName, GameStoreContext dbContext)
    {
        var genre = await dbContext.Genres
            .SingleOrDefaultAsync(genre => genre.Name == genreName);

        if (genre is not null)
        {
            return genre;
        }

        genre = new Genre { Name = genreName };
        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync();

        return genre;
    }

    private static GameDto ToDto(this Game game) =>
        new(
            game.Id,
            game.Name,
            game.Genre?.Name ?? string.Empty,
            game.Price,
            game.ReleaseDate);
}
