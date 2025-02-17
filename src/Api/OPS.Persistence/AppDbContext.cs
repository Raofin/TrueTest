using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;

namespace OPS.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CloudFile> CloudFiles { get; set; } = null!;
    public DbSet<Examination> Exams { get; set; } = null!;
    public DbSet<ExamCandidate> ExamCandidates { get; set; } = null!;
    public DbSet<FlaggedSubmission> FlaggedSolutions { get; set; } = null!;
    public DbSet<LogEvent> LogEvents { get; set; } = null!;
    public DbSet<McqOption> McqOptions { get; set; } = null!;
    public DbSet<Otp> Otps { get; set; } = null!;
    public DbSet<ProgLanguage> ProgLanguages { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionType> QuestionTypes { get; set; } = null!;
    public DbSet<RoleType> Roles { get; set; } = null!;
    public DbSet<SocialLink> Socials { get; set; } = null!;
    public DbSet<SocialType> SocialPlatforms { get; set; } = null!;
    public DbSet<TestCase> TestCases { get; set; } = null!;
    public DbSet<Account> Users { get; set; } = null!;
    public DbSet<Profile> UserDetails { get; set; } = null!;
    public DbSet<McqSubmission> UserMcqAnswers { get; set; } = null!;
    public DbSet<WrittenSubmission> UserWrittenAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        //modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}