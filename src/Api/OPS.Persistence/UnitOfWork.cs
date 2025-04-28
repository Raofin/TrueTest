using Microsoft.EntityFrameworkCore;
using OPS.Domain;
using OPS.Domain.Entities.Common;
using OPS.Domain.Interfaces.Core;
using OPS.Domain.Interfaces.Exams;
using OPS.Domain.Interfaces.Questions;
using OPS.Domain.Interfaces.Submissions;
using OPS.Domain.Interfaces.Users;

namespace OPS.Persistence;

internal class UnitOfWork(
    AppDbContext dbContext,
    IAccountRepository accountRepository,
    IAccountRoleRepository accountRoleRepository,
    IOtpRepository otpRepository,
    IExamRepository examRepository,
    IExamCandidatesRepository examCandidatesRepository,
    IQuestionRepository questionRepository,
    IWrittenSubmissionRepository writtenSubmissionRepository,
    IMcqSubmissionRepository mcqSubmissionRepository,
    IMcqOptionRepository mcqOptionRepository,
    IProfileRepository profileRepository,
    IProfileLinkRepository profileLinkRepository,
    IProblemSubmissionRepository problemSubmissionRepository,
    ITestCaseOutputRepository testCaseOutputRepository,
    ICloudFileRepository cloudFileRepository,
    ITestCaseRepository testCaseRepository,
    IAdminInviteRepository adminInviteRepository) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public IAccountRepository Account { get; } = accountRepository;
    public IAccountRoleRepository AccountRole { get; } = accountRoleRepository;
    public IOtpRepository Otp { get; } = otpRepository;
    public IExamRepository Exam { get; } = examRepository;
    public IExamCandidatesRepository ExamCandidate { get; } = examCandidatesRepository;
    public IQuestionRepository Question { get; } = questionRepository;
    public IWrittenSubmissionRepository WrittenSubmission { get; } = writtenSubmissionRepository;
    public IMcqSubmissionRepository McqSubmission { get; } = mcqSubmissionRepository;
    public IMcqOptionRepository McqOption { get; } = mcqOptionRepository;
    public IProfileRepository Profile { get; } = profileRepository;
    public IProfileLinkRepository ProfileLink { get; } = profileLinkRepository;
    public IProblemSubmissionRepository ProblemSubmission { get; } = problemSubmissionRepository;
    public ITestCaseOutputRepository TestCaseOutput { get; } = testCaseOutputRepository;
    public ITestCaseRepository TestCase { get; } = testCaseRepository;
    public ICloudFileRepository CloudFile { get; } = cloudFileRepository;
    public IAdminInviteRepository AdminInvite { get; } = adminInviteRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        _dbContext.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(entry => entry.State == EntityState.Modified)
            .ToList()
            .ForEach(entityEntry => entityEntry.Property(nameof(IBaseEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow);

        _dbContext.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(entry => entry.State == EntityState.Deleted)
            .ToList()
            .ForEach(entityEntry =>
            {
                entityEntry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
                entityEntry.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = DateTime.UtcNow;
                entityEntry.State = EntityState.Modified;
            });

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}