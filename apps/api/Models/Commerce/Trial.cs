namespace Virenza.Api.Models.Commerce;

public class Trial
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public int DurationDays { get; set; } = 7;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public bool IsUsed { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? ConvertedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
