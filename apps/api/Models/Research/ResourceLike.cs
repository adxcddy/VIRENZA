namespace Virenza.Api.Models.Research;

public class ResourceLike
{
    public Guid Id { get; set; }

    public Guid ResearchResourceId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ResearchResource? ResearchResource { get; set; }
}
