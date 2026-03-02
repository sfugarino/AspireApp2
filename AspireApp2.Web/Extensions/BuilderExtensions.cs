using AspireApp2.Web;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace AspireApp2.Web.Extensions;
public static class BuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public string RedisConnectionString => builder.Configuration.GetConnectionString("cache") ?? "192.168.1.106:6379";

        public IHostApplicationBuilder AddServices()
        {
            builder.AddOutputCache("cache")
                    .AddDataProtection()
                    .AddRazorComponents()
                    .AddApiClient();
                    
            return builder;
        }

        public IHostApplicationBuilder AddRazorComponents()
        {
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            return builder;
        }

        public IHostApplicationBuilder AddOutputCache(string cacheName)
        {
            Console.WriteLine("Redis Connection String from Configuration: " + builder.RedisConnectionString);

            builder.AddRedisOutputCache("cache", settings => 
            {
                settings.ConnectionString = builder.RedisConnectionString;
            });

            return builder;
        }
        
        public IHostApplicationBuilder AddDataProtection()
        {
            var redis = ConnectionMultiplexer.Connect(builder.RedisConnectionString);

            builder.Services.AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys")
            .SetApplicationName("AspireApp2.Web");
            return builder;
        }

        public IHostApplicationBuilder AddApiClient()
        {
            var apiServiceBaseUrl = builder.Configuration["ApiService:BaseUrl"] ?? "https://apiservice";

            Console.WriteLine("API Service Base URL: " + apiServiceBaseUrl);

            builder.Services.AddHttpClient<WeatherApiClient>(client =>
            {
                // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
                // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
                client.BaseAddress = new(apiServiceBaseUrl);
            });
            return builder;
        }
    }
}
