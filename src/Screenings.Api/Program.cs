using BuildingBlocks.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Screenings.Api.Data;
using Screenings.Api.Infrastructure.Clients.Seating;
using Screenings.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddErrorHandling();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Screening API",
        Version = "v1",
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IScreeningService, ScreeningService>();

builder.Services.Configure<SeatingClientOptions>(
    builder.Configuration.GetSection("Services:SeatingApi"));

builder.Services.AddHttpClient<ISeatingClient, SeatingClient>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<SeatingClientOptions>>().Value;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ScreeningDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Screening API v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScreeningDbContext>();
    await db.Database.MigrateAsync();
}

app.MapControllers();

app.Run();
