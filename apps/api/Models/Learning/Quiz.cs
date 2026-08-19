namespace Virenza.Api.Models.Learning;

public class Quiz
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LessonId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Instructions { get; set; }

    public int PassPercentage { get; set; } = 50;

    public int TimeLimitMinutes { get; set; }

    public bool IsPublished { get; set; }
}
