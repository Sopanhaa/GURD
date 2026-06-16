namespace GameStore.Api.Dtos;

// A dto is a contract between the client and the server.

public record GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);

// this is a class 