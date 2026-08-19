namespace Virenza.Api.Models.Learning;

public class AssessmentResult
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StudentId { get; set; }

    public Guid QuizId { get; set; }

    public decimal Score { get; set; }

    public decimal Percentage { get; set; }

    public bool Passed { get; set; }

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
