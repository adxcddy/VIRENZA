namespace Virenza.Api.Models.Commerce;

public class Subscription
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public Guid SubscriptionPlanId { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Pending;

    public DateTime StartedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
