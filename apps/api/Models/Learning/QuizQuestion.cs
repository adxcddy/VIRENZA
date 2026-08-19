namespace Virenza.Api.Models.Learning;

public class QuizQuestion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid QuizId { get; set; }

    public string Question { get; set; } = string.Empty;

    public string QuestionType { get; set; } = "MultipleChoice";

    public int Points { get; set; } = 1;

    public int Order { get; set; }
}
