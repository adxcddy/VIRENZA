using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Services.Research;

namespace Virenza.Api.Controllers.Research;

[ApiController]
[Route("api/research")]
[AllowAnonymous]
public sealed class ResearchController : ControllerBase
{
    private readonly VirenzaDbContext _db;
    private readonly IResearchService _researchService;

    public ResearchController(
        VirenzaDbContext db,
        IResearchService researchService)
    {
        _db = db;
        _researchService = researchService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] string? countryCode,
        [FromQuery] string? languageCode,
        [FromQuery] int limit = 50)
    {
        var publications = await _researchService.SearchPublicationsAsync(
            q,
            countryCode,
            languageCode,
            limit);

        var datasets = await _researchService.SearchDatasetsAsync(
            q,
            countryCode,
            languageCode,
            limit);

        return Ok(new
        {
            query = q,
            filters = new
            {
                countryCode,
                languageCode
            },
            publications,
            datasets,
            counts = new
            {
                publications = publications.Count,
                datasets = datasets.Count
            }
        });
    }

    [HttpGet("publications")]
    public async Task<IActionResult> Publications(
        [FromQuery] string? q,
        [FromQuery] string? countryCode,
        [FromQuery] string? languageCode,
        [FromQuery] int limit = 50)
    {
        return Ok(await _researchService.SearchPublicationsAsync(
            q,
            countryCode,
            languageCode,
            limit));
    }

    [HttpGet("datasets")]
    public async Task<IActionResult> Datasets(
        [FromQuery] string? q,
        [FromQuery] string? countryCode,
        [FromQuery] string? languageCode,
        [FromQuery] int limit = 50)
    {
        return Ok(await _researchService.SearchDatasetsAsync(
            q,
            countryCode,
            languageCode,
            limit));
    }

    [HttpGet("sources")]
    public async Task<IActionResult> Sources()
    {
        var sources = await _db.KnowledgeSources
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.BaseUrl,
                x.ApiUrl,
                x.SourceType,
                x.License,
                x.LicenseUrl,
                x.CountryCode,
                x.LanguageCode,
                x.LastSuccessfulSyncAt
            })
            .ToListAsync();

        return Ok(sources);
    }

    [HttpGet("topics")]
    public async Task<IActionResult> Topics()
    {
        var topics = await _db.ResearchTopics
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug,
                x.Description
            })
            .ToListAsync();

        return Ok(topics);
    }
}
