namespace Virenza.Api.Models.Curriculum;

public class AcademicYear
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CurriculumId { get; set; }

    public Guid LearningLevelId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
