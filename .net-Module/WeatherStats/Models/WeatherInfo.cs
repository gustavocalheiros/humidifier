namespace WeatherStats.Models;

public class WeatherInfo : TableEntityBase
{
    public WeatherInfo(string partitionKey, string rowKey, int temperature, int humidity) 
        : base(partitionKey, rowKey)
    {
        Temperature = temperature;
        Humidity = humidity;
    }

    public int Temperature { get; set; }

    public int Humidity { get; set; }
}
