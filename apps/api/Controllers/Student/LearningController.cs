using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Virenza.Api.Data;
using Virenza.Api.Models.Learning;

namespace Virenza.Api.Controllers.Student;

[ApiController]
[Route("api/learning")]
[Authorize]
public class LearningController : ControllerBase
{
    private readonly VirenzaDbContext _db;

    public LearningController(VirenzaDbContext db)
    {
        _db = db;
    }

    // Discover published courses
    [HttpGet("courses")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourses(
        [FromQuery] string? search,
        [FromQuery] Guid? subjectId,
        [FromQuery] Guid? learningLevelId)
    {
        var query = _db.Courses
            .AsNoTracking()
            .Where(x => x.IsPublished);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Title.Contains(search) ||
                (x.Description != null &&
                 x.Description.Contains(search)));
        }

        if (subjectId.HasValue)
            query = query.Where(x => x.SubjectId == subjectId);

        if (learningLevelId.HasValue)
            query = query.Where(x => x.LearningLevelId == learningLevelId);

        var courses = await query
            .OrderBy(x => x.Title)
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Slug,
                x.Description,
                x.Difficulty,
                x.EstimatedHours,
                x.IsFree,
                x.SubjectId,
                x.LearningLevelId
            })
            .ToListAsync();

        return Ok(courses);
    }

    // Course details
    [HttpGet("courses/{courseId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourse(Guid courseId)
    {
        var course = await _db.Courses
            .AsNoTracking()
            .Where(x => x.Id == courseId && x.IsPublished)
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Slug,
                x.Description,
                x.Difficulty,
                x.EstimatedHours,
                x.IsFree,
                x.SubjectId,
                x.LearningLevelId,

                Modules = _db.Modules
                    .Where(m => m.CourseId == x.Id && m.IsPublished)
                    .OrderBy(m => m.Order)
                    .Select(m => new
                    {
                        m.Id,
                        m.Title,
                        m.Description,
                        m.Order,

                        Lessons = _db.Lessons
                            .Where(l =>
                                l.ModuleId == m.Id &&
                                l.IsPublished)
                            .OrderBy(l => l.Order)
                            .Select(l => new
                            {
                                l.Id,
                                l.Title,
                                l.Summary,
                                l.ContentType,
                                l.EstimatedMinutes,
                                l.Order
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (course == null)
            return NotFound(new { message = "Course not found." });

        return Ok(course);
    }

    // Enroll student in a course
    [HttpPost("courses/{courseId:guid}/enroll")]
    public async Task<IActionResult> Enroll(Guid courseId)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var course = await _db.Courses
            .FirstOrDefaultAsync(x =>
                x.Id == courseId &&
                x.IsPublished);

        if (course == null)
            return NotFound(new { message = "Course not found." });

        var existing = await _db.Enrollments
            .FirstOrDefaultAsync(x =>
                x.CourseId == courseId &&
                x.StudentId == studentId.Value);

        if (existing != null)
        {
            if (!existing.IsActive)
                existing.IsActive = true;

            await _db.SaveChangesAsync();

            return Ok(existing);
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId.Value,
            CourseId = courseId,
            ProgressPercent = 0,
            IsActive = true,
            IsCompleted = false
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return Ok(enrollment);
    }

    // Student's courses
    [HttpGet("my-courses")]
    public async Task<IActionResult> MyCourses()
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var courses = await _db.Enrollments
            .AsNoTracking()
            .Where(x => x.StudentId == studentId.Value)
            .OrderByDescending(x => x.EnrolledAt)
            .Select(x => new
            {
                EnrollmentId = x.Id,
                x.CourseId,
                x.EnrolledAt,
                x.ProgressPercent,
                x.IsCompleted,
                x.CompletedAt,

                Course = _db.Courses
                    .Where(c => c.Id == x.CourseId)
                    .Select(c => new
                    {
                        c.Title,
                        c.Slug,
                        c.Difficulty,
                        c.EstimatedHours
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(courses);
    }

    // Get a lesson for the authenticated student.
    // Content is only available to students enrolled in the course.
    [HttpGet("lessons/{lessonId:guid}")]
    public async Task<IActionResult> GetLesson(Guid lessonId)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var lesson = await _db.Lessons
            .AsNoTracking()
            .Where(x =>
                x.Id == lessonId &&
                x.IsPublished)
            .Select(x => new
            {
                x.Id,
                x.ModuleId,
                x.Title,
                x.Summary,
                x.Content,
                x.ContentType,
                x.EstimatedMinutes,
                x.Order,

                Module = _db.Modules
                    .Where(m =>
                        m.Id == x.ModuleId &&
                        m.IsPublished)
                    .Select(m => new
                    {
                        m.Id,
                        m.CourseId,
                        m.Title,
                        m.Order
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (lesson == null || lesson.Module == null)
            return NotFound(new { message = "Lesson not found." });

        var enrollment = await _db.Enrollments
            .AsNoTracking()
            .AnyAsync(x =>
                x.StudentId == studentId.Value &&
                x.CourseId == lesson.Module.CourseId &&
                x.IsActive);

        if (!enrollment)
        {
            return BadRequest(new
            {
                message = "Student is not enrolled in this course."
            });
        }

        var progress = await _db.LessonProgress
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId.Value &&
                x.LessonId == lessonId);

        var navigation = await _db.Lessons
            .AsNoTracking()
            .Where(x =>
                x.IsPublished &&
                _db.Modules.Any(m =>
                    m.Id == x.ModuleId &&
                    m.CourseId == lesson.Module.CourseId &&
                    m.IsPublished))
            .OrderBy(x => x.ModuleId)
            .ThenBy(x => x.Order)
            .Select(x => new
            {
                x.Id,
                x.ModuleId,
                x.Title,
                x.Order
            })
            .ToListAsync();

        var currentIndex = navigation.FindIndex(x => x.Id == lessonId);

        return Ok(new
        {
            lesson.Id,
            lesson.ModuleId,
            lesson.Title,
            lesson.Summary,
            lesson.Content,
            lesson.ContentType,
            lesson.EstimatedMinutes,
            lesson.Order,

            lesson.Module,

            Progress = progress == null
                ? new
                {
                    ProgressPercent = 0m,
                    TimeSpentSeconds = 0,
                    IsCompleted = false,
                    CompletedAt = (DateTime?)null
                }
                : new
                {
                    progress.ProgressPercent,
                    progress.TimeSpentSeconds,
                    progress.IsCompleted,
                    progress.CompletedAt
                },

            PreviousLesson = currentIndex > 0
                ? navigation[currentIndex - 1]
                : null,

            NextLesson = currentIndex >= 0 &&
                         currentIndex < navigation.Count - 1
                ? navigation[currentIndex + 1]
                : null
        });
    }

    // Record lesson progress
    [HttpPost("lessons/{lessonId:guid}/progress")]
    public async Task<IActionResult> UpdateProgress(
        Guid lessonId,
        [FromBody] ProgressRequest request)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var lesson = await _db.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == lessonId &&
                x.IsPublished);

        if (lesson == null)
            return NotFound(new { message = "Lesson not found." });

        var module = await _db.Modules
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == lesson.ModuleId);

        if (module == null)
            return NotFound();

        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId.Value &&
                x.CourseId == module.CourseId &&
                x.IsActive);

        if (enrollment == null)
        {
            return BadRequest(new
            {
                message = "Student is not enrolled in this course."
            });
        }

        var progress = await _db.LessonProgress
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId.Value &&
                x.LessonId == lessonId);

        if (progress == null)
        {
            progress = new LessonProgress
            {
                StudentId = studentId.Value,
                LessonId = lessonId
            };

            _db.LessonProgress.Add(progress);
        }

        progress.ProgressPercent =
            Math.Clamp(request.ProgressPercent, 0, 100);

        progress.TimeSpentSeconds =
            Math.Max(0, request.TimeSpentSeconds);

        progress.LastAccessedAt = DateTime.UtcNow;

        if (progress.ProgressPercent >= 100)
        {
            progress.IsCompleted = true;
            progress.CompletedAt ??= DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();

        await RecalculateCourseProgress(
            studentId.Value,
            module.CourseId);

        return Ok(progress);
    }

    private async Task RecalculateCourseProgress(
        Guid studentId,
        Guid courseId)
    {
        var lessonIds = await _db.Lessons
            .Where(l =>
                l.IsPublished &&
                _db.Modules.Any(m =>
                    m.Id == l.ModuleId &&
                    m.CourseId == courseId &&
                    m.IsPublished))
            .Select(l => l.Id)
            .ToListAsync();

        if (lessonIds.Count == 0)
            return;

        var completedLessons = await _db.LessonProgress
            .CountAsync(p =>
                p.StudentId == studentId &&
                lessonIds.Contains(p.LessonId) &&
                p.IsCompleted);

        var percentage =
            Math.Round(
                (decimal)completedLessons / lessonIds.Count * 100,
                2);

        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId &&
                x.CourseId == courseId);

        if (enrollment == null)
            return;

        enrollment.ProgressPercent = percentage;

        if (percentage >= 100)
        {
            var publishedQuizIds = await _db.Quizzes
                .Where(q =>
                    q.IsPublished &&
                    _db.Lessons.Any(l =>
                        l.Id == q.LessonId &&
                        l.IsPublished &&
                        _db.Modules.Any(m =>
                            m.Id == l.ModuleId &&
                            m.CourseId == courseId &&
                            m.IsPublished)))
                .Select(q => q.Id)
                .ToListAsync();

            var passedQuizIds = await _db.AssessmentResults
                .Where(r =>
                    r.StudentId == studentId &&
                    r.Passed &&
                    publishedQuizIds.Contains(r.QuizId))
                .GroupBy(r => r.QuizId)
                .Select(g => g.Key)
                .ToListAsync();

            var allQuizzesPassed =
                publishedQuizIds.Count == passedQuizIds.Count;

            if (allQuizzesPassed)
            {
                enrollment.IsCompleted = true;
                enrollment.CompletedAt ??= DateTime.UtcNow;

                var certificate = await _db.Certificates
                    .FirstOrDefaultAsync(x =>
                        x.StudentId == studentId &&
                        x.CourseId == courseId);

                if (certificate == null)
                {
                    var certificateNumber =
                        $"VIR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}"[..24]
                            .ToUpperInvariant();

                    var verificationCode =
                        Guid.NewGuid()
                            .ToString("N")
                            .ToUpperInvariant();

                    _db.Certificates.Add(new Certificate
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        CertificateNumber = certificateNumber,
                        VerificationCode = verificationCode,
                        IssuedAt = DateTime.UtcNow,
                        IsValid = true
                    });
                }
                else if (!certificate.IsValid)
                {
                    certificate.IsValid = true;
                }
            }
            else
            {
                enrollment.IsCompleted = false;
                enrollment.CompletedAt = null;
            }
        }
        else
        {
            enrollment.IsCompleted = false;
            enrollment.CompletedAt = null;
        }

        await _db.SaveChangesAsync();
    }


    // Get quiz for a lesson.
    // Correct answers are intentionally never returned.
    [HttpGet("lessons/{lessonId:guid}/quiz")]
    public async Task<IActionResult> GetLessonQuiz(Guid lessonId)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var lesson = await _db.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == lessonId &&
                x.IsPublished);

        if (lesson == null)
            return NotFound(new { message = "Lesson not found." });

        var module = await _db.Modules
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == lesson.ModuleId &&
                x.IsPublished);

        if (module == null)
            return NotFound(new { message = "Module not found." });

        var enrolled = await _db.Enrollments
            .AnyAsync(x =>
                x.StudentId == studentId.Value &&
                x.CourseId == module.CourseId &&
                x.IsActive);

        if (!enrolled)
        {
            return BadRequest(new
            {
                message = "Student is not enrolled in this course."
            });
        }

        var quiz = await _db.Quizzes
            .AsNoTracking()
            .Where(x =>
                x.LessonId == lessonId &&
                x.IsPublished)
            .Select(x => new
            {
                x.Id,
                x.LessonId,
                x.Title,
                x.Instructions,
                x.PassPercentage,
                x.TimeLimitMinutes,

                Questions = _db.QuizQuestions
                    .Where(q => q.QuizId == x.Id)
                    .OrderBy(q => q.Order)
                    .Select(q => new
                    {
                        q.Id,
                        q.Question,
                        q.QuestionType,
                        q.Points,
                        q.Order,

                        Options = _db.QuizOptions
                            .Where(o => o.QuizQuestionId == q.Id)
                            .OrderBy(o => o.Order)
                            .Select(o => new
                            {
                                o.Id,
                                o.Text,
                                o.Order
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (quiz == null)
            return NotFound(new
            {
                message = "No published quiz found for this lesson."
            });

        return Ok(quiz);
    }

    // Submit quiz answers.
    [HttpPost("quizzes/{quizId:guid}/submit")]
    public async Task<IActionResult> SubmitQuiz(
        Guid quizId,
        [FromBody] QuizSubmissionRequest request)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        if (request.Answers == null || request.Answers.Count == 0)
        {
            return BadRequest(new
            {
                message = "At least one answer is required."
            });
        }

        var quiz = await _db.Quizzes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == quizId &&
                x.IsPublished);

        if (quiz == null)
            return NotFound(new { message = "Quiz not found." });

        var lesson = await _db.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == quiz.LessonId &&
                x.IsPublished);

        if (lesson == null)
            return NotFound(new { message = "Quiz lesson not found." });

        var module = await _db.Modules
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == lesson.ModuleId &&
                x.IsPublished);

        if (module == null)
            return NotFound(new { message = "Quiz module not found." });

        var enrolled = await _db.Enrollments
            .AnyAsync(x =>
                x.StudentId == studentId.Value &&
                x.CourseId == module.CourseId &&
                x.IsActive);

        if (!enrolled)
        {
            return BadRequest(new
            {
                message = "Student is not enrolled in this course."
            });
        }

        var questions = await _db.QuizQuestions
            .AsNoTracking()
            .Where(x => x.QuizId == quizId)
            .OrderBy(x => x.Order)
            .ToListAsync();

        if (questions.Count == 0)
            return BadRequest(new
            {
                message = "Quiz contains no questions."
            });

        var questionIds = questions
            .Select(x => x.Id)
            .ToHashSet();

        var submittedAnswers = request.Answers
            .Where(x => questionIds.Contains(x.QuestionId))
            .GroupBy(x => x.QuestionId)
            .ToDictionary(
                x => x.Key,
                x => x.First().OptionId);

        var optionIds = submittedAnswers
            .Values
            .ToHashSet();

        var options = await _db.QuizOptions
            .AsNoTracking()
            .Where(x => optionIds.Contains(x.Id))
            .ToListAsync();

        var optionsById = options.ToDictionary(x => x.Id);

        decimal score = 0;
        decimal totalPoints = questions.Sum(x => x.Points);

        foreach (var question in questions)
        {
            if (!submittedAnswers.TryGetValue(
                    question.Id,
                    out var selectedOptionId))
            {
                continue;
            }

            if (!optionsById.TryGetValue(
                    selectedOptionId,
                    out var selectedOption))
            {
                continue;
            }

            if (selectedOption.QuizQuestionId != question.Id)
                continue;

            if (selectedOption.IsCorrect)
                score += question.Points;
        }

        var percentage = totalPoints <= 0
            ? 0
            : Math.Round(score / totalPoints * 100, 2);

        var passed = percentage >= quiz.PassPercentage;

        var result = new AssessmentResult
        {
            StudentId = studentId.Value,
            QuizId = quizId,
            Score = score,
            Percentage = percentage,
            Passed = passed,
            SubmittedAt = DateTime.UtcNow
        };

        _db.AssessmentResults.Add(result);

        if (passed)
        {
            var progress = await _db.LessonProgress
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId.Value &&
                    x.LessonId == lesson.Id);

            if (progress == null)
            {
                progress = new LessonProgress
                {
                    StudentId = studentId.Value,
                    LessonId = lesson.Id
                };

                _db.LessonProgress.Add(progress);
            }

            progress.ProgressPercent = 100;
            progress.IsCompleted = true;
            progress.LastAccessedAt = DateTime.UtcNow;
            progress.CompletedAt ??= DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();

        if (passed)
        {
            await RecalculateCourseProgress(
                studentId.Value,
                module.CourseId);
        }

        return Ok(new
        {
            result.Id,
            result.QuizId,
            result.Score,
            result.Percentage,
            result.Passed,
            quiz.PassPercentage,
            result.SubmittedAt
        });
    }

    // Student's previous quiz results.
    [HttpGet("quizzes/{quizId:guid}/results")]
    public async Task<IActionResult> GetQuizResults(Guid quizId)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var quizExists = await _db.Quizzes
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id == quizId &&
                x.IsPublished);

        if (!quizExists)
            return NotFound(new { message = "Quiz not found." });

        var results = await _db.AssessmentResults
            .AsNoTracking()
            .Where(x =>
                x.StudentId == studentId.Value &&
                x.QuizId == quizId)
            .OrderByDescending(x => x.SubmittedAt)
            .Select(x => new
            {
                x.Id,
                x.Score,
                x.Percentage,
                x.Passed,
                x.SubmittedAt
            })
            .ToListAsync();

        return Ok(results);
    }

    // =========================================================
    // CERTIFICATES
    // =========================================================

    // Get all certificates belonging to the logged-in student.
    [HttpGet("my-certificates")]
    public async Task<IActionResult> GetMyCertificates()
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var certificates = await _db.Certificates
            .AsNoTracking()
            .Where(x =>
                x.StudentId == studentId.Value &&
                x.IsValid)
            .OrderByDescending(x => x.IssuedAt)
            .Select(x => new
            {
                x.Id,
                x.CourseId,
                x.CertificateNumber,
                x.VerificationCode,
                x.IssuedAt,
                x.IsValid,
                Course = _db.Courses
                    .Where(c => c.Id == x.CourseId)
                    .Select(c => new
                    {
                        c.Title,
                        c.Slug
                    })
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(certificates);
    }

    // Get one certificate belonging to the logged-in student.
    [HttpGet("certificates/{certificateId:guid}")]
    public async Task<IActionResult> GetCertificate(Guid certificateId)
    {
        var studentId = GetStudentId();

        if (studentId == null)
            return Unauthorized();

        var certificate = await _db.Certificates
            .AsNoTracking()
            .Where(x =>
                x.Id == certificateId &&
                x.StudentId == studentId.Value)
            .Select(x => new
            {
                x.Id,
                x.StudentId,
                x.CourseId,
                x.CertificateNumber,
                x.VerificationCode,
                x.IssuedAt,
                x.IsValid,
                Course = _db.Courses
                    .Where(c => c.Id == x.CourseId)
                    .Select(c => new
                    {
                        c.Title,
                        c.Slug
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (certificate == null)
        {
            return NotFound(new
            {
                valid = false,
                message = "Certificate not found."
            });
        }

        return Ok(certificate);
    }

    // Public certificate verification.
    // No student authentication is required.
    [AllowAnonymous]
    [HttpGet("certificates/verify/{verificationCode}")]
    public async Task<IActionResult> VerifyCertificate(string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(verificationCode))
        {
            return BadRequest(new
            {
                valid = false,
                message = "Verification code is required."
            });
        }

        var certificate = await _db.Certificates
            .AsNoTracking()
            .Where(x =>
                x.VerificationCode == verificationCode &&
                x.IsValid)
            .Select(x => new
            {
                x.CertificateNumber,
                x.VerificationCode,
                x.IssuedAt,
                x.IsValid,
                Course = _db.Courses
                    .Where(c => c.Id == x.CourseId)
                    .Select(c => new
                    {
                        c.Title,
                        c.Slug
                    })
                    .FirstOrDefault(),
                Student = _db.Users
                    .Where(u => u.Id == x.StudentId)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (certificate == null)
        {
            return NotFound(new
            {
                valid = false,
                message = "Certificate is invalid or does not exist."
            });
        }

        return Ok(new
        {
            valid = true,
            certificate
        });
    }

    private Guid? GetStudentId()
    {
        var value =
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ??
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value ??
            User.FindFirst("id")?.Value;

        return Guid.TryParse(value, out var id)
            ? id
            : null;
    }
}

public sealed class QuizSubmissionRequest
{
    public List<QuizAnswerRequest> Answers { get; set; } = new();
}

public sealed class QuizAnswerRequest
{
    public Guid QuestionId { get; set; }

    public Guid OptionId { get; set; }
}

public sealed class ProgressRequest
{
    public decimal ProgressPercent { get; set; }

    public int TimeSpentSeconds { get; set; }
}
