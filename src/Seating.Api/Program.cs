using Microsoft.EntityFrameworkCore;
using Seating.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SeatingDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello! I'm Seating service.");

app.Run();
