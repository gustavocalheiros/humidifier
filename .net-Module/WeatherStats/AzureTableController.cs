using Azure;
using Azure.Data.Tables;
using System.Timers;
using WeatherStats.KeyVault;
using WeatherStats.Models;
using Timer = System.Timers.Timer;

namespace WeatherStats;

public class AzureTableController : IDisposable
{
    private const int TimerInterval = 2000;

    private readonly LocalTableController _localTableController;
    private readonly IKeyVault _keyVault;
    private readonly Settings _settings;

    private readonly TableServiceClient _tableServiceClient;
    private readonly Timer _updateSocketStatusTimer;

    public AzureTableController(LocalTableController localTableController, IKeyVault keyVault) :
        this(localTableController, keyVault, Settings.Default)
    { }

    private AzureTableController(LocalTableController localTableController, IKeyVault keyVault, Settings settings)
    {
        _localTableController = localTableController;
        _keyVault = keyVault;
        _settings = settings;

        this._tableServiceClient = this.SetupTable().Result;

        UpdateTable();

        _updateSocketStatusTimer = new Timer(TimerInterval);

        _updateSocketStatusTimer.Elapsed += UpdateSocketStatus;
        _updateSocketStatusTimer.AutoReset = true;
        _updateSocketStatusTimer.Enabled = true;
        _updateSocketStatusTimer.Start();

    }

    private void UpdateSocketStatus(object? sender, ElapsedEventArgs e)
    {
        var tableClient = _tableServiceClient.GetTableClient(_settings.TableName);

        var entities = tableClient.Query<WeatherInfo>();
        var weatherInfos = entities.ToList();

        WeatherInfo? weatherInfo = weatherInfos.OrderBy(x => x.Timestamp).FirstOrDefault();

        if(weatherInfo?.Humidity > 35) 
        {
            SmartSocket.TurnOn(_settings.SocketKey);
        }
        else
        {
            SmartSocket.TurnOff(_settings.SocketKey);
        }
    }
    

    public async Task<TableServiceClient> SetupTable()
    {
        var accountKey = await _keyVault.GetSecretValue(_settings.AzureSecret);

        return new TableServiceClient(
           new Uri(_settings.EndPoint),
           new TableSharedKeyCredential(_settings.AccountName, accountKey.Value.Value));
    }

    public void UpdateTable()
    {
        using var context = new WeatherInfoContext();

        var tableClient = _tableServiceClient.GetTableClient(_settings.TableName);
        tableClient.CreateIfNotExists();

        foreach (var weatherInfoEf in _localTableController.ReadData(context))
        {
            var response = this.InsertData(tableClient, weatherInfoEf);
            Console.WriteLine("data inserted? " + response);
            if (!response.IsError)
            {
                var r = _localTableController.DeleteData(context, weatherInfoEf);
                Console.WriteLine("value deleted? " + r);
            }
        }

        context.SaveChanges();
    }

    public Response InsertData(TableClient tableClient, WeatherInfoEF infoEF)
    {
        var info = new WeatherInfo(_settings.PartitionKey, 
            (infoEF.Time.Ticks / TimeSpan.TicksPerSecond).ToString(), 
            infoEF.Temperature, 
            infoEF.Humidity);

        return tableClient.UpsertEntity(info);
    }

    public void Dispose()
    {
        _updateSocketStatusTimer.Elapsed -= this.UpdateSocketStatus;
        _updateSocketStatusTimer.Dispose();
    }
}