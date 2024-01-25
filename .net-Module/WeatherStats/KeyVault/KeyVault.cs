namespace WeatherStats.KeyVault;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

internal class KeyVault : IKeyVault
{
    private readonly SecretClient _client;

    public KeyVault() :
        this(Settings.Default)
    { }


    private KeyVault(Settings settings)
    {
        var defaultAzureCredential = new DefaultAzureCredential();
        //TokenCredential credential = new AzureCliCredential();

        _client = new SecretClient(new Uri(settings.AzureKeyVaultUrl), defaultAzureCredential); ;
    }

    public Task<Azure.Response<KeyVaultSecret>> GetSecretValue(string secret)
    {
        return _client.GetSecretAsync(secret);
    }
}