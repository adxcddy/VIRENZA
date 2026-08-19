using Microsoft.EntityFrameworkCore;
using Virenza.Api.Models.Curriculum;
using Virenza.Api.Models.Education;

namespace Virenza.Api.Data.Seed;

public static class VirenzaCurriculumSeed
{
    public static async Task SeedAsync(VirenzaDbContext db)
    {
        // ---------------------------------------------------------
        // COUNTRIES
        // ---------------------------------------------------------

        var uganda = await db.Countries
            .FirstOrDefaultAsync(x => x.Code == "UG");

        if (uganda == null)
        {
            uganda = new Country
            {
                Name = "Uganda",
                Code = "UG",
                IsActive = true
            };

            db.Countries.Add(uganda);
            await db.SaveChangesAsync();
        }

        // ---------------------------------------------------------
        // CURRICULUM
        // ---------------------------------------------------------

        var curriculum = await db.Curricula
            .FirstOrDefaultAsync(x =>
                x.CountryId == uganda.Id &&
                x.Name == "VIRENZA Uganda Curriculum");

        if (curriculum == null)
        {
            curriculum = new Curriculum
            {
                CountryId = uganda.Id,
                Name = "VIRENZA Uganda Curriculum",
                Description =
                    "Initial VIRENZA curriculum structure for Uganda, designed to support learning from foundation level through doctoral and lifelong learning.",
                Version = 1,
                IsActive = true
            };

            db.Curricula.Add(curriculum);
            await db.SaveChangesAsync();
        }

        // ---------------------------------------------------------
        // EDUCATION LEVELS
        // ---------------------------------------------------------

        var levelNames = new[]
        {
            "Early Learning",
            "Primary",
            "Secondary",
            "Vocational & Technical",
            "Certificate",
            "Diploma",
            "Bachelor's Degree",
            "Postgraduate Diploma",
            "Master's Degree",
            "Doctoral / PhD",
            "Research",
            "Professional & Lifelong Learning"
        };

        var levels = new Dictionary<string, LearningLevel>();

        for (var i = 0; i < levelNames.Length; i++)
        {
            var name = levelNames[i];

            var level = await db.LearningLevels
                .FirstOrDefaultAsync(x => x.Name == name);

            if (level == null)
            {
                level = new LearningLevel
                {
                    Name = name,
                    Order = i + 1,
                    Description = $"{name} learning level."
                };

                db.LearningLevels.Add(level);
                await db.SaveChangesAsync();
            }

            levels[name] = level;
        }

        // ---------------------------------------------------------
        // ACADEMIC YEARS / GRADES
        // ---------------------------------------------------------

        var gradeDefinitions = new Dictionary<string, string[]>
        {
            ["Early Learning"] =
            [
                "Nursery 1",
                "Nursery 2",
                "Nursery 3"
            ],

            ["Primary"] =
            [
                "Primary 1",
                "Primary 2",
                "Primary 3",
                "Primary 4",
                "Primary 5",
                "Primary 6",
                "Primary 7"
            ],

            ["Secondary"] =
            [
                "Senior 1",
                "Senior 2",
                "Senior 3",
                "Senior 4",
                "Senior 5",
                "Senior 6"
            ],

            ["Vocational & Technical"] =
            [
                "Foundation",
                "Intermediate",
                "Advanced"
            ],

            ["Certificate"] =
            [
                "Certificate Level 1",
                "Certificate Level 2",
                "Certificate Level 3"
            ],

            ["Diploma"] =
            [
                "Diploma Year 1",
                "Diploma Year 2",
                "Diploma Year 3"
            ],

            ["Bachelor's Degree"] =
            [
                "Bachelor's Year 1",
                "Bachelor's Year 2",
                "Bachelor's Year 3",
                "Bachelor's Year 4",
                "Bachelor's Year 5",
                "Bachelor's Year 6"
            ],

            ["Postgraduate Diploma"] =
            [
                "Postgraduate Diploma"
            ],

            ["Master's Degree"] =
            [
                "Master's Year 1",
                "Master's Year 2",
                "Master's Year 3"
            ],

            ["Doctoral / PhD"] =
            [
                "Doctoral Coursework",
                "Doctoral Research",
                "Doctoral Dissertation"
            ],

            ["Research"] =
            [
                "Research Foundation",
                "Independent Research",
                "Advanced Research"
            ],

            ["Professional & Lifelong Learning"] =
            [
                "Beginner",
                "Intermediate",
                "Advanced",
                "Professional"
            ]
        };

        foreach (var definition in gradeDefinitions)
        {
            var level = levels[definition.Key];

            for (var i = 0; i < definition.Value.Length; i++)
            {
                var name = definition.Value[i];

                var exists = await db.AcademicYears.AnyAsync(x =>
                    x.CurriculumId == curriculum.Id &&
                    x.LearningLevelId == level.Id &&
                    x.Name == name);

                if (!exists)
                {
                    db.AcademicYears.Add(new AcademicYear
                    {
                        CurriculumId = curriculum.Id,
                        LearningLevelId = level.Id,
                        Name = name,
                        Order = i + 1,
                        IsActive = true
                    });
                }
            }
        }

        await db.SaveChangesAsync();

        // ---------------------------------------------------------
        // CORE SUBJECTS
        // ---------------------------------------------------------

        var subjects = new[]
        {
            ("MAT", "Mathematics", true),
            ("ENG", "English Language", true),
            ("SCI", "Science", true),
            ("ICT", "Information & Communication Technology", false),
            ("CSC", "Computer Science", false),
            ("BIO", "Biology", false),
            ("CHE", "Chemistry", false),
            ("PHY", "Physics", false),
            ("GEO", "Geography", false),
            ("HIS", "History", false),
            ("ECO", "Economics", false),
            ("BUS", "Business Studies", false),
            ("ACC", "Accounting", false),
            ("ART", "Art & Creativity", false),
            ("AGR", "Agriculture", false),
            ("LIT", "Literature", false),
            ("LAN", "Languages", false),
            ("LAW", "Law", false),
            ("MED", "Medicine & Health Sciences", false),
            ("ENGR", "Engineering", false),
            ("EDU", "Education", false),
            ("STAT", "Statistics", false),
            ("AI", "Artificial Intelligence", false),
            ("DATA", "Data Science", false),
            ("CYB", "Cybersecurity", false),
            ("RES", "Research Methods", false)
        };

        foreach (var level in levels.Values)
        {
            foreach (var subject in subjects)
            {
                var exists = await db.AcademicSubjects.AnyAsync(x =>
                    x.CurriculumId == curriculum.Id &&
                    x.LearningLevelId == level.Id &&
                    x.Code == subject.Item1);

                if (!exists)
                {
                    db.AcademicSubjects.Add(new AcademicSubject
                    {
                        CurriculumId = curriculum.Id,
                        LearningLevelId = level.Id,
                        Code = subject.Item1,
                        Name = subject.Item2,
                        IsCore = subject.Item3,
                        IsActive = true
                    });
                }
            }
        }

        await db.SaveChangesAsync();
    }
}
