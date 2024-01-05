using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherStats.KeyVault;
using WeatherStats.Models;

namespace WeatherStats.Extensions;

public static class ServiceCollectionExtensions
{
    public static void SetupDI(HostApplicationBuilder builder, IServiceProvider? provider = null)
    {
        builder.Services.AddSingleton<WeatherInfoContext>();
        builder.Services.AddSingleton<IKeyVault, KeyVault.KeyVault>();
        builder.Services.AddSingleton<WeatherStats>();

        builder.Services.AddTransient<LocalTableController>();
        builder.Services.AddTransient<AzureTableController>();
    }
}
