namespace WeatherStats.Models;

using Microsoft.EntityFrameworkCore;

public class WeatherInfoContext : DbContext
{
    private static readonly DateTime UnitTimeZero = new(1970, 1, 1, 0, 0, 0, 0);

    public DbSet<WeatherInfoEF> WeatherInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=..\\Weather_Stats.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Id auto-increment
        modelBuilder.Entity<WeatherInfoEF>().Property(p => p.Id).ValueGeneratedOnAdd();

        // seconds to DateTime
        modelBuilder.Entity<WeatherInfoEF>().Property(p => p.Time).HasConversion(
            v => v.Second,
            v => UnitTimeZero.AddSeconds(v));
    }
}
