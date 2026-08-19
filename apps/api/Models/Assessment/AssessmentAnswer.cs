namespace Virenza.Api.Models.Assessment;

public class AssessmentAnswer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AttemptId { get; set; }

    public Guid QuestionId { get; set; }

    public Guid? SelectedOptionId { get; set; }

    public string? AnswerText { get; set; }

    public decimal AwardedPoints { get; set; }

    public bool IsCorrect { get; set; }

    public string? Feedback { get; set; }
}
