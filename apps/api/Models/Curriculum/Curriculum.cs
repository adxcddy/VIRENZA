namespace Virenza.Api.Models.Curriculum;

public class Curriculum
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CountryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Version { get; set; } = 1;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
