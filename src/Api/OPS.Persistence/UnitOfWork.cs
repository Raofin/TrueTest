using Microsoft.EntityFrameworkCore;
using OPS.Domain;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Common;
using OPS.Persistence.Repositories;

namespace OPS.Persistence;

internal class UnitOfWork(AppDbContext dbContext, IExamRepository examRepository, IAccountRepository accountRepository, IExamCandidatesRepository examCandidateRepository)
    : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;

    public IExamRepository Exam { get; } = examRepository;
    public IAccountRepository Account { get; } = accountRepository;
    public IExamCandidatesRepository ExamCandiates { get; } = examCandidateRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var softDeletableEntity = _dbContext.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entityEntry in softDeletableEntity)
        {
            entityEntry.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
            entityEntry.State = EntityState.Modified;
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}