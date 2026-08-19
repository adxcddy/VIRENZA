namespace Virenza.Api.Models.Curriculum;

public class EducationGrade
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LearningLevelId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
