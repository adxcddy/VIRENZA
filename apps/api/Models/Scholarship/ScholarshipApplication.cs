namespace Virenza.Api.Models.Scholarship;

public class ScholarshipApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ScholarshipId { get; set; }

    public Guid StudentUserId { get; set; }

    public string? Motivation { get; set; }

    public ScholarshipApplicationStatus Status { get; set; }
        = ScholarshipApplicationStatus.Pending;

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }
}
