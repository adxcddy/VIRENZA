namespace Virenza.Api.Models.Learning;

public class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SubjectId { get; set; }

    public Guid InstructorId { get; set; }

    public Guid LearningLevelId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Difficulty { get; set; } = "Beginner";

    public int EstimatedHours { get; set; }

    public bool IsPublished { get; set; }

    public bool IsFree { get; set; }

    public Guid? PrerequisiteCourseId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? PublishedAt { get; set; }
}
