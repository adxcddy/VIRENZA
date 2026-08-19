namespace Virenza.Api.Models.Education;

public class LearningLevel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public int Order { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
