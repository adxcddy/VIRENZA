namespace Virenza.Api.Models.Assessment;

public class Assessment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CourseId { get; set; }

    public Guid? ModuleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string AssessmentType { get; set; } = "Quiz";

    public string? Instructions { get; set; }

    public decimal MaximumScore { get; set; } = 100;

    public decimal PassPercentage { get; set; } = 50;

    public int? TimeLimitMinutes { get; set; }

    public int AttemptsAllowed { get; set; } = 1;

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableUntil { get; set; }
}
