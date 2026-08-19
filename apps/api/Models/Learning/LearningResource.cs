namespace Virenza.Api.Models.Learning;

public class LearningResource
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LessonId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ResourceType { get; set; } = "Document";

    public string Url { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsFree { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
