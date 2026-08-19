namespace Virenza.Api.Models.Identity;

public class StudentProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public string? Country { get; set; }

    public string? Bio { get; set; }

    public string? LearningGoal { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
