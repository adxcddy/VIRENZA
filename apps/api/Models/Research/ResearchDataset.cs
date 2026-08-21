namespace Virenza.Api.Models.Research;

public class ResearchDataset
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SourceId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Publisher { get; set; }

    public string? ExternalId { get; set; }

    public string? DatasetUrl { get; set; }

    public string? APIUrl { get; set; }

    public string? License { get; set; }

    public string? LicenseUrl { get; set; }

    public string? LanguageCode { get; set; }

    public string? CountryCode { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public KnowledgeSource? Source { get; set; }
}
