using WeatherStats.KeyVault;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherStats;

internal class Program
{
    private static void Main()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Services.AddTransient<WeatherStats>(); //transient?
        builder.Services.AddSingleton<IKeyVault, KeyVault.KeyVault>();

        using IHost host = builder.Build();

        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var weatherStats = provider.GetRequiredService<WeatherStats>();

        weatherStats.SetupTable();

        host.Run();
    }
}