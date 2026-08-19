namespace Virenza.Api.Models.Learning;

public class Certificate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public string CertificateNumber { get; set; } = string.Empty;

    public string VerificationCode { get; set; } = string.Empty;

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public bool IsValid { get; set; } = true;
}
