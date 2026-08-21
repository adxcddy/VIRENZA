using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Curriculum;
using Virenza.Api.Models.Education;
using Virenza.Api.Models.Identity;
using Virenza.Api.Models.Learning;

namespace Virenza.Api.Data.Seed;

public static class VirenzaLearningSeed
{
    public static async Task SeedAsync(VirenzaDbContext db)
    {
        // =========================================================
        // 1. ENSURE A TEACHER EXISTS
        // =========================================================

        var teacher = await db.Users
            .FirstOrDefaultAsync(x =>
                x.Email == "teacher@virenza.local");

        if (teacher == null)
        {
            teacher = new User
            {
                FirstName = "VIRENZA",
                LastName = "Teacher",
                Email = "teacher@virenza.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
                Role = UserRole.Teacher,
                IsActive = true,
                EmailVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Users.Add(teacher);
            await db.SaveChangesAsync();
        }
        else if (teacher.Role != UserRole.Teacher)
        {
            teacher.Role = UserRole.Teacher;
            teacher.IsActive = true;
            teacher.EmailVerified = true;

            await db.SaveChangesAsync();
        }

        // =========================================================
        // 2. ENSURE TEACHER PROFILE EXISTS
        // =========================================================

        var teacherProfile = await db.TeacherProfiles
            .FirstOrDefaultAsync(x => x.UserId == teacher.Id);

        if (teacherProfile == null)
        {
            db.TeacherProfiles.Add(new TeacherProfile
            {
                UserId = teacher.Id,
                Biography =
                    "VIRENZA educator focused on accessible, practical and technology-enabled learning.",
                Expertise =
                    "Mathematics, ICT, Computer Science and Digital Learning",
                Qualifications =
                    "Professional Educator",
                Country = "Uganda",
                IsVerified = true,
                CreatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }

        // =========================================================
        // 3. ENSURE SUBJECTS EXIST
        // =========================================================

        var subjectDefinitions = new[]
        {
            ("MAT", "Mathematics", "mathematics"),
            ("ENG", "English Language", "english-language"),
            ("SCI", "Science", "science"),
            ("ICT", "Information & Communication Technology", "information-communication-technology"),
            ("CSC", "Computer Science", "computer-science"),
            ("BIO", "Biology", "biology"),
            ("CHE", "Chemistry", "chemistry"),
            ("PHY", "Physics", "physics"),
            ("GEO", "Geography", "geography"),
            ("HIS", "History", "history"),
            ("ECO", "Economics", "economics"),
            ("BUS", "Business Studies", "business-studies"),
            ("ACC", "Accounting", "accounting"),
            ("ART", "Art & Creativity", "art-creativity"),
            ("AGR", "Agriculture", "agriculture"),
            ("LIT", "Literature", "literature"),
            ("LAN", "Languages", "languages"),
            ("LAW", "Law", "law"),
            ("MED", "Medicine & Health Sciences", "medicine-health-sciences"),
            ("ENGR", "Engineering", "engineering"),
            ("EDU", "Education", "education"),
            ("STAT", "Statistics", "statistics"),
            ("AI", "Artificial Intelligence", "artificial-intelligence"),
            ("DATA", "Data Science", "data-science"),
            ("CYB", "Cybersecurity", "cybersecurity"),
            ("RES", "Research Methods", "research-methods")
        };

        foreach (var subject in subjectDefinitions)
        {
            var exists = await db.Subjects
                .AnyAsync(x => x.Slug == subject.Item3);

            if (!exists)
            {
                db.Subjects.Add(new Subject
                {
                    Name = subject.Item2,
                    Slug = subject.Item3,
                    Description = $"{subject.Item2} learning and educational content.",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await db.SaveChangesAsync();

        // =========================================================
        // 4. FIND LEARNING LEVELS
        // =========================================================

        var primary = await db.LearningLevels
            .FirstAsync(x => x.Name == "Primary");

        var secondary = await db.LearningLevels
            .FirstAsync(x => x.Name == "Secondary");

        var vocational = await db.LearningLevels
            .FirstAsync(x => x.Name == "Vocational & Technical");

        // =========================================================
        // 5. CREATE COURSES
        // =========================================================

        var mathematics = await db.Subjects
            .FirstAsync(x => x.Slug == "mathematics");

        var english = await db.Subjects
            .FirstAsync(x => x.Slug == "english-language");

        var science = await db.Subjects
            .FirstAsync(x => x.Slug == "science");

        var ict = await db.Subjects
            .FirstAsync(x => x.Slug == "information-communication-technology");

        var computerScience = await db.Subjects
            .FirstAsync(x => x.Slug == "computer-science");

        var courses = new[]
        {
            new
            {
                SubjectId = mathematics.Id,
                LearningLevelId = primary.Id,
                Title = "Primary Mathematics Foundations",
                Slug = "primary-mathematics-foundations",
                Description = "A foundation course covering essential primary mathematics concepts.",
                Difficulty = "Beginner",
                EstimatedHours = 20,
                IsFree = true
            },
            new
            {
                SubjectId = english.Id,
                LearningLevelId = primary.Id,
                Title = "Primary English Language",
                Slug = "primary-english-language",
                Description = "Build essential English reading, writing, grammar and communication skills.",
                Difficulty = "Beginner",
                EstimatedHours = 18,
                IsFree = true
            },
            new
            {
                SubjectId = science.Id,
                LearningLevelId = primary.Id,
                Title = "Primary Science",
                Slug = "primary-science",
                Description = "Explore foundational science concepts through practical learning.",
                Difficulty = "Beginner",
                EstimatedHours = 20,
                IsFree = true
            },
            new
            {
                SubjectId = mathematics.Id,
                LearningLevelId = secondary.Id,
                Title = "Secondary Mathematics",
                Slug = "secondary-mathematics",
                Description = "Develop strong mathematical reasoning and problem-solving skills.",
                Difficulty = "Intermediate",
                EstimatedHours = 30,
                IsFree = true
            },
            new
            {
                SubjectId = science.Id,
                LearningLevelId = secondary.Id,
                Title = "Secondary Science",
                Slug = "secondary-science",
                Description = "Core secondary science concepts and scientific reasoning.",
                Difficulty = "Intermediate",
                EstimatedHours = 28,
                IsFree = true
            },
            new
            {
                SubjectId = ict.Id,
                LearningLevelId = secondary.Id,
                Title = "ICT Fundamentals",
                Slug = "ict-fundamentals",
                Description = "Learn computer fundamentals, digital literacy and responsible technology use.",
                Difficulty = "Beginner",
                EstimatedHours = 15,
                IsFree = true
            },
            new
            {
                SubjectId = computerScience.Id,
                LearningLevelId = vocational.Id,
                Title = "Computer Science Foundations",
                Slug = "computer-science-foundations",
                Description = "Introduction to programming, algorithms, data structures and computational thinking.",
                Difficulty = "Intermediate",
                EstimatedHours = 35,
                IsFree = true
            }
        };

        foreach (var definition in courses)
        {
            var course = await db.Courses
                .FirstOrDefaultAsync(x => x.Slug == definition.Slug);

            if (course == null)
            {
                course = new Course
                {
                    SubjectId = definition.SubjectId,
                    InstructorId = teacher.Id,
                    LearningLevelId = definition.LearningLevelId,
                    Title = definition.Title,
                    Slug = definition.Slug,
                    Description = definition.Description,
                    Difficulty = definition.Difficulty,
                    EstimatedHours = definition.EstimatedHours,
                    IsPublished = true,
                    IsFree = definition.IsFree,
                    CreatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                };

                db.Courses.Add(course);
                await db.SaveChangesAsync();
            }

            // =====================================================
            // 6. MODULES
            // =====================================================

            var moduleDefinitions = new[]
            {
                (
                    "Introduction",
                    "Introduction and essential concepts.",
                    1
                ),
                (
                    "Core Concepts",
                    "Core concepts and guided examples.",
                    2
                ),
                (
                    "Practical Application",
                    "Apply the concepts through practical activities.",
                    3
                ),
                (
                    "Assessment & Review",
                    "Review, practice and assessment.",
                    4
                )
            };

            foreach (var moduleDefinition in moduleDefinitions)
            {
                var module = await db.Modules
                    .FirstOrDefaultAsync(x =>
                        x.CourseId == course.Id &&
                        x.Order == moduleDefinition.Item3);

                if (module == null)
                {
                    module = new Module
                    {
                        CourseId = course.Id,
                        Title = moduleDefinition.Item1,
                        Description = moduleDefinition.Item2,
                        Order = moduleDefinition.Item3,
                        IsPublished = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    db.Modules.Add(module);
                    await db.SaveChangesAsync();
                }

                // =================================================
                // 7. LESSONS
                // =================================================

                var lessons = new[]
                {
                    (
                        "Getting Started",
                        "Introduction to the module.",
                        "Welcome to this module. In this lesson you will learn the key ideas you need before continuing.",
                        10,
                        1
                    ),
                    (
                        "Understanding the Concepts",
                        "Explore the main concepts.",
                        "This lesson explains the central concepts using clear examples and guided explanations.",
                        15,
                        2
                    ),
                    (
                        "Practice Activity",
                        "Apply what you have learned.",
                        "Complete the practice activity and apply the concepts to real learning situations.",
                        20,
                        3
                    )
                };

                foreach (var lessonDefinition in lessons)
                {
                    var exists = await db.Lessons.AnyAsync(x =>
                        x.ModuleId == module.Id &&
                        x.Order == lessonDefinition.Item5);

                    if (!exists)
                    {
                        db.Lessons.Add(new Lesson
                        {
                            ModuleId = module.Id,
                            Title = lessonDefinition.Item1,
                            Summary = lessonDefinition.Item2,
                            Content = lessonDefinition.Item3,
                            ContentType = "Text",
                            EstimatedMinutes = lessonDefinition.Item4,
                            Order = lessonDefinition.Item5,
                            IsPublished = true,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                await db.SaveChangesAsync();
            }
        }

        await SeedQuizzesAsync(db);
    }


    private static async Task SeedQuizzesAsync(VirenzaDbContext db)
    {
        var lessons = await db.Lessons
            .Where(x => x.IsPublished)
            .ToListAsync();

        foreach (var lesson in lessons)
        {
            var existingQuiz = await db.Quizzes
                .FirstOrDefaultAsync(x => x.LessonId == lesson.Id);

            if (existingQuiz != null)
                continue;

            var quiz = new Quiz
            {
                LessonId = lesson.Id,
                Title = $"{lesson.Title} Quiz",
                Instructions =
                    "Answer all questions. Select the best answer for each question.",
                PassPercentage = 50,
                TimeLimitMinutes = 10,
                IsPublished = true
            };

            db.Quizzes.Add(quiz);
            await db.SaveChangesAsync();

            var questions = new[]
            {
                new
                {
                    Question = $"What is the main purpose of the lesson \"{lesson.Title}\"?",
                    Correct = "To understand and apply the key concepts introduced in the lesson.",
                    Wrong1 = "To skip the learning material.",
                    Wrong2 = "To avoid practicing the concepts.",
                    Wrong3 = "To replace the entire course.",
                    Order = 1
                },
                new
                {
                    Question = "Which approach is most useful when learning a new concept?",
                    Correct = "Understand the concept, practice it and apply it.",
                    Wrong1 = "Memorize without understanding.",
                    Wrong2 = "Skip examples and practice.",
                    Wrong3 = "Avoid asking questions.",
                    Order = 2
                },
                new
                {
                    Question = "What should a student do after completing a learning activity?",
                    Correct = "Review the result and identify areas that need improvement.",
                    Wrong1 = "Ignore the result.",
                    Wrong2 = "Delete the work immediately.",
                    Wrong3 = "Stop learning completely.",
                    Order = 3
                }
            };

            foreach (var definition in questions)
            {
                var question = new QuizQuestion
                {
                    QuizId = quiz.Id,
                    Question = definition.Question,
                    QuestionType = "MultipleChoice",
                    Points = 1,
                    Order = definition.Order
                };

                db.QuizQuestions.Add(question);
                await db.SaveChangesAsync();

                db.QuizOptions.AddRange(
                    new QuizOption
                    {
                        QuizQuestionId = question.Id,
                        Text = definition.Correct,
                        IsCorrect = true,
                        Order = 1
                    },
                    new QuizOption
                    {
                        QuizQuestionId = question.Id,
                        Text = definition.Wrong1,
                        IsCorrect = false,
                        Order = 2
                    },
                    new QuizOption
                    {
                        QuizQuestionId = question.Id,
                        Text = definition.Wrong2,
                        IsCorrect = false,
                        Order = 3
                    },
                    new QuizOption
                    {
                        QuizQuestionId = question.Id,
                        Text = definition.Wrong3,
                        IsCorrect = false,
                        Order = 4
                    }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}

