using BuildingBlocks.ErrorHandling;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Screening.Api.Data;

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

app.MapControllers();

app.Run();
