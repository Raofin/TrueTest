using Microsoft.EntityFrameworkCore;
using OPS.Domain;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Common;

namespace OPS.Persistence;

internal class UnitOfWork(
    AppDbContext dbContext,
    IAccountRepository accountRepository,
    IOtpRepository otpRepository,
    IExamRepository examRepository) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public IAccountRepository Account { get; } = accountRepository;
    public IOtpRepository Otp { get; } = otpRepository;
    public IExamRepository Exam { get; } = examRepository;
    public IExamCandidatesRepository ExamCandiates { get; } = examCandidateRepository;
    public IQuestionRepository Question { get; } = questionRepository;
    public IWrittenSubmissionRepository WrittenSubmission { get; } = writtenSubmissionRepository; 


    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var softDeletableEntities = _dbContext.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entityEntry in softDeletableEntities)
        {
            entityEntry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
        }

        var updatedEntities = _dbContext.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(entry => entry.State == EntityState.Modified)
            .ToList();

        foreach (var entityEntry in updatedEntities) entityEntry.Property(nameof(IBaseEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}