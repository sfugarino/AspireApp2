namespace AspireApp2.ApiService.Config;

public class WeatherDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ForecastCollectionName { get; set; } = null!;
}