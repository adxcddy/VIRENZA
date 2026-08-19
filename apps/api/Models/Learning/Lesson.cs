namespace Virenza.Api.Models.Learning;

public class Lesson
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ModuleId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Summary { get; set; }

    public string Content { get; set; } = string.Empty;

    public string ContentType { get; set; } = "Text";

    public int EstimatedMinutes { get; set; }

    public int Order { get; set; }

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
