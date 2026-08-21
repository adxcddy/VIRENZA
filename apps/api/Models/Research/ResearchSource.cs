namespace Virenza.Api.Models.Research;

public class ResearchSource
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? LogoUrl { get; set; }

    public string? CountryCode { get; set; }

    public bool IsVerified { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
