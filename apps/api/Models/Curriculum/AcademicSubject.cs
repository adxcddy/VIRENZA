namespace Virenza.Api.Models.Curriculum;

public class AcademicSubject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CurriculumId { get; set; }

    public Guid LearningLevelId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsCore { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
