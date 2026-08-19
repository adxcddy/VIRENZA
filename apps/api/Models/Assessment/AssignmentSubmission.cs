namespace Virenza.Api.Models.Assessment;

public class AssignmentSubmission
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssignmentId { get; set; }

    public Guid StudentId { get; set; }

    public string? Content { get; set; }

    public string? FileUrl { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public decimal? Score { get; set; }

    public string? Feedback { get; set; }

    public bool IsGraded { get; set; }
}
