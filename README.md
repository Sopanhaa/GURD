# GameStore.Api

A minimal ASP.NET Core Web API for managing games and genres, backed by Entity Framework Core with SQL Server.

## Project Structure

- `GameStore.Api/` - ASP.NET Core Web API project
- `GameStore.Api/Data/` - EF Core DbContext and seed data
- `GameStore.Api/Endpoint/` - minimal API endpoint definitions
- `GameStore.Api/Models/` - domain models for `Game` and `Genre`
- `GameStore.Api/Dtos/` - data transfer objects for API input/output
- `wwwroot/` - static files served by the app

## Requirements

- .NET 10 SDK
- SQL Server instance

## Configuration

The project reads a connection string named `GameStore` from `appsettings.json`.
Update the connection string in `GameStore.Api/appsettings.json` to point to your SQL Server database.

Example:

```json
{
  "ConnectionStrings": {
    "GameStore": "Server=.;Database=GameStore;Trusted_Connection=True;"
  }
}
```

## Running the App

From the `GameStore.Api` folder:

```powershell
cd "\GameStore.Api"
dotnet run
```

The application will:

- configure EF Core with SQL Server
- seed initial data if needed
- serve static files from `wwwroot`
- expose game and genre endpoints

## API Endpoints

The project uses minimal API endpoints registered in `GameStore.Api/Endpoint`.

### Games

- `GET /games` - list all games
- `GET /games/{id}` - get details for a single game
- `POST /games` - create a new game
- `PUT /games/{id}` - update an existing game
- `DELETE /games/{id}` - delete a game

### Genres

- `GET /genres` - list all genres
- `GET /genres/{id}` - get details for a single genre
- `POST /genres` - create a new genre
- `PUT /genres/{id}` - update an existing genre
- `DELETE /genres/{id}` - delete a genre

## Notes

- Static web assets are served under `wwwroot`
- Data seeding is executed at startup via `SeedData.InitializeAsync`
- The app uses implicit global usings and nullable reference types enabled in `GameStore.Api.csproj`

## Development

- Add database migrations and update the database using EF Core CLI if needed.
- Update DTO or model classes to extend the API.


