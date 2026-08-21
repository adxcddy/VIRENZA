namespace Virenza.Api.Models.Research;

public class ResearchResource
{
    public Guid Id { get; set; }

    public Guid ResearchSourceId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? ResourceType { get; set; }

    public string? Url { get; set; }

    public string? Author { get; set; }

    public string? Publisher { get; set; }

    public string? Subject { get; set; }

    public string? Language { get; set; }

    public string? CountryCode { get; set; }

    public DateTime? PublishedAt { get; set; }

    public bool IsVerified { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ResearchSource? ResearchSource { get; set; }
}
