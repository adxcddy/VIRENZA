using Virenza.Api.Models.Research;

namespace Virenza.Api.Services.Research;

public interface IResearchService
{
    Task<IReadOnlyList<ResearchPublication>> SearchPublicationsAsync(
        string? query,
        string? countryCode,
        string? languageCode,
        int limit = 50);

    Task<IReadOnlyList<ResearchDataset>> SearchDatasetsAsync(
        string? query,
        string? countryCode,
        string? languageCode,
        int limit = 50);
}
