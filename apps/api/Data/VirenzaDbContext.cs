using Microsoft.EntityFrameworkCore;
using Virenza.Api.Models.Commerce;
using Virenza.Api.Models.Curriculum;
using Virenza.Api.Models.Education;
using Virenza.Api.Models.Identity;
using Virenza.Api.Models.Learning;
using Virenza.Api.Models.Assessment;
using Virenza.Api.Models.Scholarship;
using Virenza.Api.Models.Sponsorship;
using Virenza.Api.Models.Research;

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

    // Research & Global Knowledge
    // Global research ingestion
    public DbSet<KnowledgeSource> KnowledgeSources => Set<KnowledgeSource>();
    public DbSet<ResearchPublication> ResearchPublications => Set<ResearchPublication>();
    public DbSet<ResearchDataset> ResearchDatasets => Set<ResearchDataset>();
    public DbSet<ResearchTopic> ResearchTopics => Set<ResearchTopic>();

    // Curated research resources
    public DbSet<ResearchSource> ResearchSources => Set<ResearchSource>();
    public DbSet<ResearchResource> ResearchResources => Set<ResearchResource>();
    public DbSet<ResourceLike> ResourceLikes => Set<ResourceLike>();
    public DbSet<ResourceBookmark> ResourceBookmarks => Set<ResourceBookmark>();

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

        modelBuilder.Entity<ResearchSource>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(x => x.WebsiteUrl)
                .HasMaxLength(1000);

            entity.Property(x => x.CountryCode)
                .HasMaxLength(10);
        });

        modelBuilder.Entity<ResearchResource>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Url)
                .HasMaxLength(2000);

            entity.Property(x => x.ResourceType)
                .HasMaxLength(100);

            entity.Property(x => x.Language)
                .HasMaxLength(20);

            entity.Property(x => x.CountryCode)
                .HasMaxLength(10);

            entity.HasOne(x => x.ResearchSource)
                .WithMany()
                .HasForeignKey(x => x.ResearchSourceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.ResearchSourceId);
            entity.HasIndex(x => x.Subject);
            entity.HasIndex(x => x.Language);
            entity.HasIndex(x => x.CountryCode);
        });

        modelBuilder.Entity<ResourceLike>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.ResearchResource)
                .WithMany()
                .HasForeignKey(x => x.ResearchResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.ResearchResourceId,
                x.UserId
            })
            .IsUnique();
        });

        modelBuilder.Entity<ResourceBookmark>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.ResearchResource)
                .WithMany()
                .HasForeignKey(x => x.ResearchResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new
            {
                x.ResearchResourceId,
                x.UserId
            })
            .IsUnique();
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
