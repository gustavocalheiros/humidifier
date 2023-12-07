namespace WeatherStats;

using Azure;
using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;

public class WeatherContext : DbContext
{
    public DbSet<WeatherInfo> WeatherInfos { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=..\\Weather_Stats.db");
    }
}

public class WeatherInfo : ITableEntity
{
    public WeatherInfo(string partitionKey, string rowKey, int temperature, int humidity)
    {
        PartitionKey = partitionKey;
        RowKey = rowKey;
        Temperature = temperature;
        Humidity = humidity;
    }

    public string PartitionKey { get; set; }
    
    public string RowKey { get; set; }
    
    public DateTimeOffset? Timestamp { get; set; }
    
    public ETag ETag { get; set; }
    
    public int Temperature { get; }
    
    public int Humidity { get; }
}