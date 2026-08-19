namespace Virenza.Api.Models.Sponsorship;

public class SponsorshipRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StudentUserId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string? PersonalStatement { get; set; }

    public string? Country { get; set; }

    public string? EducationLevel { get; set; }

    public SponsorshipRequestStatus Status { get; set; }
        = SponsorshipRequestStatus.Submitted;

    public Guid? ReviewedByUserId { get; set; }

    public Guid? SponsorId { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public DateTime? SponsoredAt { get; set; }
}
