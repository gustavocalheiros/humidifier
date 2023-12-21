namespace WeatherStats;

public class SmartSocket
{
    private static readonly HttpClient Client = new();
    private const string WebhookUrlPlaceholder = "https://maker.ifttt.com/trigger/{0}/with/key/{1}";

    public static Task<string> TurnOn(string key)
    {
        var webhookUrl = string.Format(WebhookUrlPlaceholder, "turn_on_socket", key);
        return Client.GetStringAsync(webhookUrl);
    }

    public static Task<string> TurnOff(string key)
    {
        var webhookUrl = string.Format(WebhookUrlPlaceholder, "turn_off_socket", key);
        return Client.GetStringAsync(webhookUrl);
    }
}
