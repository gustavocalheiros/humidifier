namespace WeatherStats.KeyVault;

using Azure.Security.KeyVault.Secrets;

public interface IKeyVault
{
    Task<Azure.Response<KeyVaultSecret>> GetSecretValue(string secret);
}