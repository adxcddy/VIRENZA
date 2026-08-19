namespace Virenza.Api.Models.Commerce;

public class Donation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? DonorUserId { get; set; }

    public string? DonorName { get; set; }

    public string? DonorEmail { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = CurrencyCode.USD;

    public string? Message { get; set; }

    public bool IsAnonymous { get; set; }

    public string? Provider { get; set; }

    public string? ProviderTransactionId { get; set; }

    public DonationStatus Status { get; set; } = DonationStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }
}
