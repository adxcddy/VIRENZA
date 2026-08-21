namespace Virenza.Api.Models.Research;

public class KnowledgeSource
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string BaseUrl { get; set; } = string.Empty;

    public string? ApiUrl { get; set; }

    public string SourceType { get; set; } = "OpenData";

    public string? License { get; set; }

    public string? LicenseUrl { get; set; }

    public string? CountryCode { get; set; }

    public string? LanguageCode { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastSyncedAt { get; set; }

    public DateTime? LastSuccessfulSyncAt { get; set; }
}
