namespace Virenza.Api.Configuration.Payments;

public class AirtelPaymentOptions
{
    public bool Enabled { get; set; }

    public string BaseUrl { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Environment { get; set; } = "sandbox";
}
