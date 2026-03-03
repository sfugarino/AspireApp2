using AspireApp2.ApiService.Config;
using AspireApp2.ApiService.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AspireApp2.ApiService.Services;

public class WeatherService
{
    private readonly IMongoCollection<WeatherForecast> _weatheForcastCollection;

    public WeatherService(
        IOptions<WeatherDatabaseSettings> weatherDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            weatherDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            weatherDatabaseSettings.Value.DatabaseName);

        _weatheForcastCollection = mongoDatabase.GetCollection<WeatherForecast>(
            weatherDatabaseSettings.Value.ForecastCollectionName);
    }

    public async Task<List<WeatherForecast>> GetAsync() =>
        await _weatheForcastCollection.Find(_ => true).ToListAsync();

    public async Task<WeatherForecast?> GetAsync(string id) =>
        await _weatheForcastCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(WeatherForecast newWeatherForecast) =>
        await _weatheForcastCollection.InsertOneAsync(newWeatherForecast);

    public async Task UpdateAsync(string id, WeatherForecast updatedWeatherForecast) =>
        await _weatheForcastCollection.ReplaceOneAsync(x => x.Id == id, updatedWeatherForecast);

    public async Task RemoveAsync(string id) =>
        await _weatheForcastCollection.DeleteOneAsync(x => x.Id == id);
}