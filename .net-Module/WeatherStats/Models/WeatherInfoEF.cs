namespace WeatherStats.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("weather_stats")]
public class WeatherInfoEF
{
    public WeatherInfoEF(DateTime time, int temperature, int humidity)
    {
        Time = time;
        Temperature = temperature;
        Humidity = humidity;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("time")]
    public DateTime Time { get; set; }

    [Column("temperature")]
    public int Temperature { get; set; }

    [Column("humidity")]
    public int Humidity { get; set; }
}