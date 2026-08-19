namespace Virenza.Api.Models.Learning;

public class QuizOption
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid QuizQuestionId { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int Order { get; set; }
}
