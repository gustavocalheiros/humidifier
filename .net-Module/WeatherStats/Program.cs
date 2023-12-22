using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherStats.Extensions;

namespace WeatherStats;

internal class Program
{
    //docker no python
    //nuke na build
    //UTs
    //https://maker.ifttt.com/trigger/turn_off_socket/with/key/pxVxph9VI0X6ASnjpo3El
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