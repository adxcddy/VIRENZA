using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Research;

namespace Virenza.Api.Services.Research;

public sealed class ResearchService : IResearchService
{
    private readonly VirenzaDbContext _db;

    public ResearchService(VirenzaDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ResearchPublication>> SearchPublicationsAsync(
        string? query,
        string? countryCode,
        string? languageCode,
        int limit = 50)
    {
        limit = Math.Clamp(limit, 1, 100);

        var publications = _db.ResearchPublications
            .AsNoTracking()
            .Include(x => x.Source)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();

            publications = publications.Where(x =>
                x.Title.Contains(term) ||
                (x.Abstract != null && x.Abstract.Contains(term)) ||
                (x.Publisher != null && x.Publisher.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(countryCode))
        {
            publications = publications.Where(x =>
                x.CountryCode == countryCode);
        }

        if (!string.IsNullOrWhiteSpace(languageCode))
        {
            publications = publications.Where(x =>
                x.LanguageCode == languageCode);
        }

        return await publications
            .OrderByDescending(x => x.PublishedAt ?? DateTime.MinValue)
            .ThenBy(x => x.Title)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ResearchDataset>> SearchDatasetsAsync(
        string? query,
        string? countryCode,
        string? languageCode,
        int limit = 50)
    {
        limit = Math.Clamp(limit, 1, 100);

        var datasets = _db.ResearchDatasets
            .AsNoTracking()
            .Include(x => x.Source)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();

            datasets = datasets.Where(x =>
                x.Title.Contains(term) ||
                (x.Description != null && x.Description.Contains(term)) ||
                (x.Publisher != null && x.Publisher.Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(countryCode))
        {
            datasets = datasets.Where(x =>
                x.CountryCode == countryCode);
        }

        if (!string.IsNullOrWhiteSpace(languageCode))
        {
            datasets = datasets.Where(x =>
                x.LanguageCode == languageCode);
        }

        return await datasets
            .OrderByDescending(x => x.PublishedAt ?? DateTime.MinValue)
            .ThenBy(x => x.Title)
            .Take(limit)
            .ToListAsync();
    }
}
