namespace Virenza.Api.Models.Assessment;

public class AssessmentQuestion
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AssessmentId { get; set; }

    public string QuestionText { get; set; } = string.Empty;

    public string QuestionType { get; set; } = "MultipleChoice";

    public decimal Points { get; set; } = 1;

    public int Order { get; set; }

    public string? Explanation { get; set; }
}
