namespace Virenza.Api.Models.Assessment;

public class Assignment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CourseId { get; set; }

    public Guid? ModuleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Instructions { get; set; }

    public decimal MaximumScore { get; set; } = 100;

    public decimal PassPercentage { get; set; } = 50;

    public DateTime? DueAt { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
