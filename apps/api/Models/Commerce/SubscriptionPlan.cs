namespace Virenza.Api.Models.Commerce;

public class SubscriptionPlan
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Default/base currency for this plan.
    /// VIRENZA currently uses UGX 10,000 as the starting weekly price.
    /// </summary>
    public string Currency { get; set; } = CurrencyCode.UGX;

    public decimal Price { get; set; } = 10000m;

    public int DurationDays { get; set; } = 7;

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
