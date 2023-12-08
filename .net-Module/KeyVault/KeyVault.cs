namespace WeatherStats.KeyVault;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

internal class KeyVault : IKeyVault
{
    private const string Url = "https://weather-stats-keyvault.vault.azure.net";
    private readonly SecretClient _client;

    public KeyVault()
    {
        var defaultAzureCredential = new DefaultAzureCredential();
        //TokenCredential credential = new AzureCliCredential();

        _client = new SecretClient(new Uri(Url), defaultAzureCredential);
    }

    public Task<Azure.Response<KeyVaultSecret>> GetSecretValue(string secret)
    {
        return _client.GetSecretAsync(secret);
    }
}