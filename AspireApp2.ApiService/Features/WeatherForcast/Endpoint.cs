using AspireApp2.ApiService.Models;
using AspireApp2.ApiService.Services;
using FastEndpoints;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StackExchange.Redis;
using System.Text.Json;

namespace AspireApp2.ApiService.Features.WeatherForcast
{
    public class Endpoint : EndpointWithoutRequest<Response>
    {
        private readonly IDatabase db;
        private readonly WeatherService weatherService;
        public Endpoint(IConnectionMultiplexer redis, WeatherService weatherService) 
        {
            this.db = redis.GetDatabase();
            this.weatherService = weatherService;
        }

        public override void Configure()
        {
            Get("/weatherforecast");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? cachedData = await db.StringGetAsync("weatherforecast");

            Response response = new Response();

            if (!string.IsNullOrEmpty(cachedData))
            {
                // 2. Deserialize the JSON string to a C# object
                response = JsonSerializer.Deserialize<Response>(cachedData) ?? new Response();
            }
            else
            {
                var forecasts = await weatherService.GetAsync();
                response = new Response(forecasts);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(response);
                TimeSpan expiry = TimeSpan.FromMinutes(5);
                await db.StringSetAsync("weatherforecast", jsonString, expiry);
            }

            await Send.OkAsync(response, ct);
        }
    }
}
