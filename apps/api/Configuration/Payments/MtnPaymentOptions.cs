namespace Virenza.Api.Configuration.Payments;

public class MtnPaymentOptions
{
    public bool Enabled { get; set; }

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiUser { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string SubscriptionKey { get; set; } = string.Empty;

    public string Environment { get; set; } = "sandbox";

    public string TargetEnvironment { get; set; } = "sandbox";
}
