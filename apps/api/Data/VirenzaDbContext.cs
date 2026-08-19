using Microsoft.EntityFrameworkCore;
using Virenza.Api.Models.Commerce;
using Virenza.Api.Models.Curriculum;
using Virenza.Api.Models.Education;
using Virenza.Api.Models.Identity;
using Virenza.Api.Models.Learning;
using Virenza.Api.Models.Assessment;
using Virenza.Api.Models.Scholarship;
using Virenza.Api.Models.Sponsorship;

namespace Virenza.Api.Data;

public class VirenzaDbContext : DbContext
{
    public VirenzaDbContext(DbContextOptions<VirenzaDbContext> options)
        : base(options)
    {
    }

    // Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();

    // Education
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Curriculum> Curricula => Set<Curriculum>();
    public DbSet<EducationGrade> EducationGrades => Set<EducationGrade>();
    public DbSet<AcademicSubject> AcademicSubjects => Set<AcademicSubject>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();

    public DbSet<LearningLevel> LearningLevels => Set<LearningLevel>();

    // Learning
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
public DbSet<Enrollment> Enrollments => Set<Enrollment>();
public DbSet<LessonProgress> LessonProgress => Set<LessonProgress>();
public DbSet<LearningResource> LearningResources => Set<LearningResource>();
public DbSet<Quiz> Quizzes => Set<Quiz>();
public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
public DbSet<QuizOption> QuizOptions => Set<QuizOption>();
public DbSet<AssessmentResult> AssessmentResults => Set<AssessmentResult>();
public DbSet<Certificate> Certificates => Set<Certificate>();

    // Commerce
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Trial> Trials => Set<Trial>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Donation> Donations => Set<Donation>();

    // Sponsorship
    public DbSet<Sponsor> Sponsors => Set<Sponsor>();
    public DbSet<Scholarship> Scholarships => Set<Scholarship>();
    public DbSet<ScholarshipApplication> ScholarshipApplications => Set<ScholarshipApplication>();
    public DbSet<SponsorshipRequest> SponsorshipRequests => Set<SponsorshipRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            entity.Property(x => x.PasswordHash)
                .IsRequired();
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<StudentProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TeacherProfile>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<TeacherProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
        });

        modelBuilder.Entity<LearningLevel>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Order)
                .IsUnique();

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.Property(x => x.Title)
                .HasMaxLength(250)
                .IsRequired();

            entity.HasOne<Subject>()
                .WithMany()
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<LearningLevel>()
                .WithMany()
                .HasForeignKey(x => x.LearningLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne<Course>()
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne<Module>()
                .WithMany()
                .HasForeignKey(x => x.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrganizationName)
                .HasMaxLength(250)
                .IsRequired();
        });

        modelBuilder.Entity<Scholarship>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(x => x.FundingAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();
        });

        modelBuilder.Entity<ScholarshipApplication>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }
}
