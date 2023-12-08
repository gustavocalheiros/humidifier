namespace WeatherStats.Models;

public class WeatherInfo : TableEntityBase
{
    public int Temperature { get; set; }

    public int Humidity { get; set; }
}
