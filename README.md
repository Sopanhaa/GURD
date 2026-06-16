# GURD / GameStore.Api

A minimal ASP.NET Core Web API for a game store.

## Project structure

- `GameStore.Api/` - ASP.NET Core project
- `GameStore.Api/Program.cs` - app startup and endpoint registration
- `GameStore.Api/Data/GameStoreContext.cs` - EF Core DbContext
- `GameStore.Api/Data/SeedData.cs` - database creation and seed data
- `GameStore.Api/Endpoint/GameEndpoint.cs` - game CRUD endpoints
- `GameStore.Api/Endpoint/GenresEndpoints.cs` - genre CRUD endpoints
- `GameStore.Api/appsettings.Development.json` - SQL Server LocalDB connection string

## Database

This project uses SQL Server LocalDB with the connection string in `GameStore.Api/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "GameStore": "Server=(localdb)\\MSSQLLocalDB;Database=GameStore;Trusted_Connection=True;TrustServerCertificate=True"
}
```

The database is created automatically when the app starts, and initial genres and games are seeded.

## Run locally

From the project folder:

```powershell
cd "c:\Users\Sopanha L\Documents\C#\GURD\GameStore.Api"
dotnet restore
dotnet build
dotnet run --urls http://localhost:5000
```

Then open:

- `http://localhost:5000/games`
- `http://localhost:5000/genres`

## API endpoints

- `GET /games`
- `GET /games/{id}`
- `POST /games`
- `PUT /games/{id}`
- `DELETE /games/{id}`
- `GET /genres`
- `GET /genres/{id}`
- `POST /genres`
- `PUT /genres/{id}`
- `DELETE /genres/{id}`

## GitHub

Pushed to: https://github.com/Sopanhaa/GURD.git
