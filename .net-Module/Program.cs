namespace WeatherStats;

using Azure.Core;
using Azure.Data.Tables;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

internal class Program
{
    private static void Main()
    {
        using var context = new WeatherContext();
        WeatherInfo info = new("1", "2", 98, 99);
        context.WeatherInfos.Add(info);
        context.SaveChanges();

        SetupTable();


        Thread.Sleep(90000);
    }

    public static async void SetupTable()
    {
        var endPoint = "https://weatherstats.table.core.windows.net/stats";
        var accountName = "weatherstats";
        var partitionKey = "Temperature_Humidity";
        var tableName = "stats";

        var accountKey = await SetupKeyVault();


        var serviceClient = new TableServiceClient(
            new Uri(endPoint),
            new TableSharedKeyCredential(accountName, accountKey.Value.Value));

        var temperature = new WeatherInfo(partitionKey, "0", 12, 13);

        var tableClient = serviceClient.GetTableClient(tableName);

        var ifNotExists = tableClient.CreateIfNotExists();

        var upsertEntity = tableClient.UpsertEntity(temperature);
    }


    public static Task<Azure.Response<KeyVaultSecret>> SetupKeyVault()
    {
        string secretName = "azure-table-secret";
        var kvUri = "https://weather-stats-keyvault.vault.azure.net";

        var defaultAzureCredential = new DefaultAzureCredential();
        //TokenCredential credential = new AzureCliCredential();

        var client = new SecretClient(new Uri(kvUri), defaultAzureCredential);
        return client.GetSecretAsync(secretName);
    }
}