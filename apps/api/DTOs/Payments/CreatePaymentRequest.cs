namespace Virenza.Api.DTOs.Payments;

public class CreatePaymentRequest
{
    public decimal Amount { get; set; }

    public string Currency { get; set; } = "UGX";

    public string Provider { get; set; } = string.Empty;

    public string Purpose { get; set; } = "Subscription";

    public string? PhoneNumber { get; set; }

    public string? Description { get; set; }
}
