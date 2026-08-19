namespace Virenza.Api.Models.Commerce;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "UGX";

    public string Provider { get; set; } = string.Empty;

    public PaymentPurpose Purpose { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public string? PhoneNumber { get; set; }

    public string? ProviderTransactionId { get; set; }

    public string? ProviderReference { get; set; }

    public string? ExternalReference { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public string? FailureReason { get; set; }
}
