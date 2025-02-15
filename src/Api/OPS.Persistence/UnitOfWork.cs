using Microsoft.EntityFrameworkCore;
using OPS.Domain;
using OPS.Domain.Contracts.Repository;
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