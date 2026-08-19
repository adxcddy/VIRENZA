namespace Virenza.Api.Models.Scholarship;

public class Scholarship
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SponsorId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal? FundingAmount { get; set; }

    public string Currency { get; set; } = "USD";

    public int AvailableSlots { get; set; }

    public DateTime ApplicationDeadline { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
