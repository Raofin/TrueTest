using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Core;
using OPS.Domain.Entities.Enum;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.Submit;
using OPS.Domain.Entities.User;
using OPS.Persistence.Seeding;

namespace OPS.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountRole> AccountRoles { get; set; } = null!;
    public DbSet<Otp> Otps { get; set; } = null!;
    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<ProfileSocial> ProfileSocials { get; set; } = null!;
    public DbSet<Difficulty> Difficulties { get; set; } = null!;
    public DbSet<ProgLanguage> ProgLanguages { get; set; } = null!;
    public DbSet<QuestionType> QuestionTypes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Examination> Examinations { get; set; } = null!;
    public DbSet<ExamCandidate> ExamCandidates { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<McqOption> McqOptions { get; set; } = null!;
    public DbSet<TestCase> TestCases { get; set; } = null!;
    public DbSet<McqSubmission> McqSubmissions { get; set; } = null!;
    public DbSet<ProblemSubmission> ProblemSubmissions { get; set; } = null!;
    public DbSet<WrittenSubmission> WrittenSubmissions { get; set; } = null!;
    public DbSet<CloudFile> CloudFiles { get; set; } = null!;
    public DbSet<LogEvent> LogEvents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.SeedRequiredData();

        base.OnModelCreating(modelBuilder);
    }
}