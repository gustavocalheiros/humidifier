namespace WeatherStats.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WeatherContext : DbContext
{
    public DbSet<WeatherInfoEF> WeatherInfos { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=..\\Weather_Stats.db");
    }

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<WeatherInfoEF>().Property(p => p.Id).ValueGeneratedOnAdd();
    }
}

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