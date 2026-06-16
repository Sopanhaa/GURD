using GameStore.Api.Data;
using GameStore.Api.Endpoint;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GameStore")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    await SeedData.InitializeAsync(dbContext);
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGameEndpoints();
app.MapGenresEndpoints();

app.Run();


