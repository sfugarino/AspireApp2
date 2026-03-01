using System.Reflection.Metadata.Ecma335;
using AspireApp2.Web;
using AspireApp2.Web.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

var redisConnectionString = builder.Configuration.GetConnectionString("cache") ?? "192.168.1.106:6379";
Console.WriteLine("Redis Connection String from Configuration: " + redisConnectionString);

builder.AddRedisOutputCache("cache", settings => 
{
    settings.ConnectionString = redisConnectionString;
});


var redis = ConnectionMultiplexer.Connect(redisConnectionString);

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys")
    .SetApplicationName("AspireApp2.Web");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiServiceBaseUrl = builder.Configuration["ApiService:BaseUrl"] ?? "https+http://apiservice";

Console.WriteLine("API Service Base URL: " + apiServiceBaseUrl);

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new(apiServiceBaseUrl);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
