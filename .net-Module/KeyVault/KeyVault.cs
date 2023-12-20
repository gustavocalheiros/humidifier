using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WeatherStats_Tests, PublicKey=002400000480000094" +
                              "0000000602000000240000525341310004000" +
                              "001000100bf8c25fcd44838d87e245ab35bf7" +
                              "3ba2615707feea295709559b3de903fb95a93" +
                              "3d2729967c3184a97d7b84c7547cd87e435b5" +
                              "6bdf8621bcb62b59c00c88bd83aa62c4fcdd4" +
                              "712da72eec2533dc00f8529c3a0bbb4103282" +
                              "f0d894d5f34e9f0103c473dce9f4b457a5dee" +
                              "fd8f920d8681ed6dfcb0a81e96bd9b176525a" +
                              "26e0b3")]

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