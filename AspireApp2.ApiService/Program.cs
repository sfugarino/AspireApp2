using AspireApp2.ApiService.Config;
using AspireApp2.ApiService.Services;
using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

try
{
    builder.Configuration.GetConnectionString("cache");
    builder.AddRedisClient("cache", settings =>
    {   
        Console.WriteLine("Redis Connection String: " + settings.ConnectionString);
        settings.ConnectionString = builder.Configuration.GetConnectionString("cache");
    });
}
catch (Exception ex)
{
    Console.WriteLine("Error retrieving Redis connection string: " + ex.Message);
}

builder.Services.Configure<WeatherDatabaseSettings>(
    builder.Configuration.GetSection("WeatherForcastDatabase"));

builder.Services.AddSingleton<WeatherService>();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseFastEndpoints();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.Run();


