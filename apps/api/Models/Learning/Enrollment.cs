namespace Virenza.Api.Models.Learning;

public class Enrollment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }

    public decimal ProgressPercent { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsCompleted { get; set; }
}
