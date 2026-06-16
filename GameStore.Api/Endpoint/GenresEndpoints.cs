using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoint;

public static class GenresEndpoints
{
    public static WebApplication MapGenresEndpoints(this WebApplication app)
    {
        app.MapGet("/genres", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                .AsNoTracking()
                .Select(genre => genre.ToDto())
                .ToListAsync())
            .WithName("GetGenres");

        app.MapGet("/genres/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres.FindAsync(id);
            return genre is not null ? Results.Ok(genre.ToDto()) : Results.NotFound();
        })
        .WithName("GetGenre");

        app.MapPost("/genres", async (GenreDto newGenre, GameStoreContext dbContext) =>
        {
            var genre = new Genre { Name = newGenre.Name };

            dbContext.Genres.Add(genre);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetGenre", new { id = genre.Id }, genre.ToDto());
        });

        app.MapPut("/genres/{id}", async (int id, GenreDto updatedGenre, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }

            genre.Name = updatedGenre.Name;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/genres/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var genre = await dbContext.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.NotFound();
            }

            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        return app;
    }

    private static GenreDto ToDto(this Genre genre) => new(genre.Id, genre.Name);
}
