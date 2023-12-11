namespace WeatherStats.Models;

using Microsoft.EntityFrameworkCore;

public class WeatherInfoContext : DbContext
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
