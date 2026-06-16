namespace GameStore.Api.Dtos;

public record GameDetailsDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string? Description = null
);
