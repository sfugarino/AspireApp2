using AspireApp2.ApiService.Models;

public record Response
{
    public Response()
    {
    }

    public Response(List<AspireApp2.ApiService.Entities.WeatherForecast> forecasts)
    {
        if(forecasts != null)
        {
            forecasts.ForEach(f =>
            {
                var forecast = new WeatherForecast(DateOnly.Parse(f.Date), f.TemperatureC, f.Summary);
                Forecasts = Forecasts.Append(forecast).ToArray();
            });
        }
    }

    public WeatherForecast[] Forecasts { get; set; } = [];
}