namespace Virenza.Api.Models.Research;

public class ResearchAuthor
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = string.Empty;

    public string? ORCID { get; set; }

    public string? Affiliation { get; set; }

    public string? CountryCode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
