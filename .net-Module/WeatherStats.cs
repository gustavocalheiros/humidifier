using WeatherStats.KeyVault;
using WeatherStats.Models;

namespace WeatherStats;

using Azure.Data.Tables;

public class WeatherStats
{
    private readonly IKeyVault _keyVault;

    public WeatherStats(IKeyVault keyVault) => _keyVault = keyVault;

    public async void SetupTable()
    {
        var endPoint = "https://weatherstats.table.core.windows.net/stats";
        var accountName = "weatherstats";
        var azureSecret = "azure-table-secret";
        var tableName = "stats";

        var accountKey = await _keyVault.GetSecretValue(azureSecret);


        var serviceClient = new TableServiceClient(
            new Uri(endPoint),
            new TableSharedKeyCredential(accountName, accountKey.Value.Value));

        using var context = new WeatherContext();
        var infoEF = new WeatherInfoEF(DateTime.Now, 98, 99);
        context.WeatherInfos.Add(infoEF);
        context.SaveChanges();

        var info = new WeatherInfo
        {
            PartitionKey = "Temperature_Humidity",
            RowKey = Guid.NewGuid().ToString("N"),
            Temperature = infoEF.Temperature,
            Humidity = infoEF.Humidity
        };


        var tableClient = serviceClient.GetTableClient(tableName);

        var ifNotExists = tableClient.CreateIfNotExists();

        var upsertEntity = tableClient.UpsertEntity(info);
    }


    //public static Task<Azure.Response<KeyVaultSecret>> SetupKeyVault()
    //{
    //    string secretName = "azure-table-secret";
    //    //string secretName = "IFTTT-key";
    //    var kvUri = "https://weather-stats-keyvault.vault.azure.net";

    //    var defaultAzureCredential = new DefaultAzureCredential();
    //    //TokenCredential credential = new AzureCliCredential();

    //    var client = new SecretClient(new Uri(kvUri), defaultAzureCredential);
    //    return client.GetSecretAsync(secretName);
    //}
}