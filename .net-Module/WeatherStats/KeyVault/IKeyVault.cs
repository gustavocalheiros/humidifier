namespace WeatherStats.KeyVault;

using Azure.Security.KeyVault.Secrets;

internal interface IKeyVault
{
    Task<Azure.Response<KeyVaultSecret>> GetSecretValue(string secret);
}