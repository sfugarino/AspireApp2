using AspireApp2.ApiService.Models;
using FastEndpoints;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StackExchange.Redis;
using System.Text.Json;

namespace AspireApp2.ApiService.Features
{
    public class Endpoint : EndpointWithoutRequest<WeatherForecast[]>
    {
        private readonly IDatabase db;
        public Endpoint(IConnectionMultiplexer redis) 
        {
            this.db = redis.GetDatabase();
        }

        public override void Configure()
        {
            Get("/weatherforecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? cachedData = await db.StringGetAsync("weatherforecast");

            WeatherForecast[] forecast = [];

            if (!string.IsNullOrEmpty(cachedData))
            {
                // 2. Deserialize the JSON string to a C# object
                forecast = JsonSerializer.Deserialize<WeatherForecast[]>(cachedData) ?? [];
            }
            else
            {
                string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

                forecast = [..Enumerable.Range(1, 5).Select(index =>
                                new WeatherForecast
                                (
                                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                    Random.Shared.Next(-20, 55),
                                    summaries[Random.Shared.Next(summaries.Length)]
                                ))
                                .ToArray()];

                string jsonString = System.Text.Json.JsonSerializer.Serialize(forecast);
                TimeSpan expiry = TimeSpan.FromMinutes(5);
                await db.StringSetAsync("weatherforecast", jsonString, expiry);
            }

            await Send.OkAsync(forecast, ct);
        }
    }
}
