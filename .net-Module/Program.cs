using WeatherStats.KeyVault;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherStats.Extensions;

namespace WeatherStats;

internal class Program
{
    private static void Main()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        ServiceCollectionExtensions.SetupDI(builder);

        using IHost host = builder.Build();

        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;

        var weatherStats = provider.GetRequiredService<WeatherStats>();

        //weatherStats.SetupTable();

        host.Run();
    }
}