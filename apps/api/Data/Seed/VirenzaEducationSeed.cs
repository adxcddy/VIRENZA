using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Education;

namespace Virenza.Api.Data.Seed;

public static class VirenzaEducationSeed
{
    public static async Task SeedAsync(VirenzaDbContext db)
    {
        var levels = new[]
        {
            new LearningLevel
            {
                Name = "Early Learning",
                Order = 1,
                Description = "Early childhood and foundation learning."
            },
            new LearningLevel
            {
                Name = "Primary",
                Order = 2,
                Description = "Primary and elementary education."
            },
            new LearningLevel
            {
                Name = "Secondary",
                Order = 3,
                Description = "Lower and upper secondary education."
            },
            new LearningLevel
            {
                Name = "Vocational & Technical",
                Order = 4,
                Description = "Technical, vocational and practical skills."
            },
            new LearningLevel
            {
                Name = "Certificate",
                Order = 5,
                Description = "Certificate-level professional and academic education."
            },
            new LearningLevel
            {
                Name = "Diploma",
                Order = 6,
                Description = "Diploma-level education and professional training."
            },
            new LearningLevel
            {
                Name = "Bachelor's Degree",
                Order = 7,
                Description = "Undergraduate university education."
            },
            new LearningLevel
            {
                Name = "Postgraduate Diploma",
                Order = 8,
                Description = "Advanced postgraduate professional education."
            },
            new LearningLevel
            {
                Name = "Master's Degree",
                Order = 9,
                Description = "Advanced postgraduate study."
            },
            new LearningLevel
            {
                Name = "Doctoral / PhD",
                Order = 10,
                Description = "Doctoral and PhD-level education."
            },
            new LearningLevel
            {
                Name = "Research",
                Order = 11,
                Description = "Independent and advanced research."
            },
            new LearningLevel
            {
                Name = "Professional & Lifelong Learning",
                Order = 12,
                Description = "Continuous professional development and lifelong learning."
            }
        };

        foreach (var level in levels)
        {
            var exists = await db.LearningLevels
                .AnyAsync(x => x.Name == level.Name);

            if (!exists)
            {
                db.LearningLevels.Add(level);
            }
        }

        await db.SaveChangesAsync();
    }
}
