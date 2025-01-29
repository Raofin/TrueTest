using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Usr;

namespace OPS.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CloudFile> CloudFiles { get; set; } = null!;
    public DbSet<Examination> Exams { get; set; } = null!;
    public DbSet<ExamCandidate> ExamCandidates { get; set; } = null!;
    public DbSet<FlaggedSolution> FlaggedSolutions { get; set; } = null!;
    public DbSet<LogEvent> LogEvents { get; set; } = null!;
    public DbSet<McqOption> McqOptions { get; set; } = null!;
    public DbSet<Otp> Otps { get; set; } = null!;
    public DbSet<Problem> Problems { get; set; } = null!;
    public DbSet<ProgLanguage> ProgLanguages { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<QuestionType> QuestionTypes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Social> Socials { get; set; } = null!;
    public DbSet<SocialPlatform> SocialPlatforms { get; set; } = null!;
    public DbSet<TestCase> TestCases { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserDetail> UserDetails { get; set; } = null!;
    public DbSet<UserMcqAnswer> UserMcqAnswers { get; set; } = null!;
    public DbSet<UserWrittenAnswer> UserWrittenAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        //modelBuilder.Seed();

        base.OnModelCreating(modelBuilder);
    }
}
