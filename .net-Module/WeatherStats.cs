using System.Timers;
using Azure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WeatherStats.KeyVault;
using WeatherStats.Models;
using Timer = System.Timers.Timer;

namespace WeatherStats;

using Azure.Data.Tables;

public class WeatherStats
{
    private const string EndPoint = "https://weatherstats.table.core.windows.net/stats";
    private const string AccountName = "weatherstats";
    private const string AzureSecret = "azure-table-secret";
    private const string TableName = "stats";
    private const string PartitionKey = "Temperature_Humidity";
    private const int TimerInterval = 2000;

    private readonly IKeyVault _keyVault;

    private readonly TableServiceClient _tableServiceClient;
    private readonly Timer _updateSocketStatusTimer;


    public WeatherStats(IKeyVault keyVault)
    {
        _keyVault = keyVault;
        this._tableServiceClient = this.SetupTable().Result;

        UpdateTables();

        _updateSocketStatusTimer = new Timer(TimerInterval);

        _updateSocketStatusTimer.Elapsed += UpdateSocketStatus;
        _updateSocketStatusTimer.AutoReset = false;
        _updateSocketStatusTimer.Enabled = true;
        _updateSocketStatusTimer.Start();

    }

    private void UpdateSocketStatus(object? sender, ElapsedEventArgs e)
    {
        var tableClient = _tableServiceClient.GetTableClient(TableName);

        var entities = tableClient.Query<WeatherInfo>();
        var weatherInfos = entities.ToList();
    }

    public async Task<TableServiceClient> SetupTable()
    {
        var accountKey = await _keyVault.GetSecretValue(AzureSecret);

         return new TableServiceClient(
            new Uri(EndPoint),
            new TableSharedKeyCredential(AccountName, accountKey.Value.Value));
    }

    public void UpdateTables()
    {
        using var context = new WeatherInfoContext();

        var tableClient = _tableServiceClient.GetTableClient(TableName);
        tableClient.CreateIfNotExists();

        foreach (var weatherInfoEf in this.ReadLocalData(context))
        {
            var response = this.InsertData(tableClient, weatherInfoEf);
            Console.WriteLine("data inserted? " + response);
            if (response.IsError == false)
            {
                var r = this.DeleteLocalData(context, weatherInfoEf);
                Console.WriteLine("value deleted? " + r);
            }
        }

        context.SaveChanges();
    }

    public void InsertLocalData()
    {
        using var context = new WeatherInfoContext();
        var infoEF = new WeatherInfoEF(DateTime.Now, 98, 99);
        context.WeatherInfos.Add(infoEF);
        context.SaveChanges();
    }

    public List<WeatherInfoEF> ReadLocalData(WeatherInfoContext context)
    {
        return context.WeatherInfos.ToList();
    }

    public Response InsertData(TableClient tableClient, WeatherInfoEF infoEF)
    {
        var info = new WeatherInfo
        {
            PartitionKey = PartitionKey,
            RowKey = (infoEF.Time.Ticks / TimeSpan.TicksPerSecond).ToString(),
            Temperature = infoEF.Temperature,
            Humidity = infoEF.Humidity
        };

        return tableClient.UpsertEntity(info);
    }

    public EntityEntry<WeatherInfoEF> DeleteLocalData(WeatherInfoContext weatherContext, WeatherInfoEF infoEF)
    {
        return weatherContext.Remove(infoEF);
    }
}