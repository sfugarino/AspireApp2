using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspireApp2.ApiService.Entities;

public class WeatherForecast
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("date")]
    public string Date { get; set; } = null!;

    [BsonElement("temperatureC")]
    public int TemperatureC { get; set; }

    [BsonElement("summary")]
    public string Summary { get; set; } = null!;
}