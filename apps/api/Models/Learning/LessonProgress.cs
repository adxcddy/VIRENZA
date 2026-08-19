namespace Virenza.Api.Models.Learning;

public class LessonProgress
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StudentId { get; set; }

    public Guid LessonId { get; set; }

    public bool IsCompleted { get; set; }

    public decimal ProgressPercent { get; set; }

    public int TimeSpentSeconds { get; set; }

    public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }
}
