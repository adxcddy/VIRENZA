namespace Virenza.Api.Models.Research;

public class ResearchInstitution
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Type { get; set; }

    public string? CountryCode { get; set; }

    public string? Website { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
