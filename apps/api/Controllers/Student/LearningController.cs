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

        var completed = await _db.LessonProgress
            .CountAsync(p =>
                p.StudentId == studentId &&
                lessonIds.Contains(p.LessonId) &&
                p.IsCompleted);

        var percentage =
            Math.Round((decimal)completed / lessonIds.Count * 100, 2);

        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(x =>
                x.StudentId == studentId &&
                x.CourseId == courseId);

        if (enrollment == null)
            return;

        enrollment.ProgressPercent = percentage;

        if (percentage >= 100)
        {
            enrollment.IsCompleted = true;
            enrollment.CompletedAt ??= DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }

    private Guid? GetStudentId()
    {
        var value =
            User.FindFirst("sub")?.Value ??
            User.FindFirst("userId")?.Value ??
            User.FindFirst("id")?.Value;

        return Guid.TryParse(value, out var id)
            ? id
            : null;
    }
}

public sealed class ProgressRequest
{
    public decimal ProgressPercent { get; set; }

    public int TimeSpentSeconds { get; set; }
}
