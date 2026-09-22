using BuildingBlocks.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SeatingDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IHallService, HallService>();

builder.Services.AddErrorHandling();

var app = builder.Build();

app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SeatingDbContext>();
    await db.Database.MigrateAsync();
}

app.MapControllers();

app.Run();
