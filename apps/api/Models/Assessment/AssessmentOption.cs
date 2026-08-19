namespace Virenza.Api.Models.Assessment;

public class AssessmentOption
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentQuestionId { get; set; }

    public string Text { get; set; } = string.Empty;

    public bool IsCorrect { get; set; }

    public int Order { get; set; }
}
