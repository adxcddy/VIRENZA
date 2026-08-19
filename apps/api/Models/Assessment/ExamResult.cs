namespace Virenza.Api.Models.Assessment;

public class ExamResult
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentId { get; set; }

    public Guid StudentId { get; set; }

    public decimal Score { get; set; }

    public decimal Percentage { get; set; }

    public string Grade { get; set; } = string.Empty;

    public bool Passed { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
}
