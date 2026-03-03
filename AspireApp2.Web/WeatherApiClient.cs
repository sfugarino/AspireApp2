namespace AspireApp2.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetFromJsonAsync<Response>("/weatherforecast", cancellationToken);
        WeatherForecast[] forecasts = response?.Forecasts?.ToArray() ?? [];

        return forecasts;
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record Response
{
    public WeatherForecast[] Forecasts { get; set; } = [];
}