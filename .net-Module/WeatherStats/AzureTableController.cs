using Azure;
using Azure.Data.Tables;
using System.Timers;
using WeatherStats.KeyVault;
using WeatherStats.Models;
using Timer = System.Timers.Timer;

namespace WeatherStats;

public class AzureTableController
{
    private const string EndPoint = "https://weatherstats.table.core.windows.net/stats";
    private const string AccountName = "weatherstats";
    private const string AzureSecret = "azure-table-secret";
    private const string TableName = "stats";
    private const string PartitionKey = "Temperature_Humidity";
    private const int TimerInterval = 2000;

    private readonly LocalTableController _localTableController;
    private readonly IKeyVault _keyVault;

    private readonly TableServiceClient _tableServiceClient;
    private readonly Timer _updateSocketStatusTimer;


    public AzureTableController(LocalTableController localTableController, IKeyVault keyVault)
    {
        _localTableController = localTableController;
        _keyVault = keyVault;
        this._tableServiceClient = this.SetupTable().Result;

        UpdateTable();

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

    public void UpdateTable()
    {
        using var context = new WeatherInfoContext();

        var tableClient = _tableServiceClient.GetTableClient(TableName);
        tableClient.CreateIfNotExists();

        foreach (var weatherInfoEf in _localTableController.ReadData(context))
        {
            var response = this.InsertData(tableClient, weatherInfoEf);
            Console.WriteLine("data inserted? " + response);
            if (response.IsError == false)
            {
                var r = _localTableController.DeleteData(context, weatherInfoEf);
                Console.WriteLine("value deleted? " + r);
            }
        }

        context.SaveChanges();
    }

    public Response InsertData(TableClient tableClient, WeatherInfoEF infoEF)
    {
        var info = new WeatherInfo
        {
            PartitionKey = PartitionKey,
            RowKey = Guid.NewGuid().ToString("N"),
            Temperature = infoEF.Temperature,
            Humidity = infoEF.Humidity
        };

        return tableClient.UpsertEntity(info);
    }
}