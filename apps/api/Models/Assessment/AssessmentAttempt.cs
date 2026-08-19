namespace Virenza.Api.Models.Assessment;

public class AssessmentAttempt
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentId { get; set; }

    public Guid StudentId { get; set; }

    public int AttemptNumber { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? SubmittedAt { get; set; }

    public decimal Score { get; set; }

    public decimal Percentage { get; set; }

    public bool Passed { get; set; }

    public bool IsGraded { get; set; }
}
