namespace Virenza.Api.Models.Identity;

public class TeacherProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public string? Biography { get; set; }

    public string? Expertise { get; set; }

    public string? Qualifications { get; set; }

    public string? Country { get; set; }

    public bool IsVerified { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
