using OPS.Domain;
using OPS.Domain.Contracts;

namespace OPS.Persistence;

internal class UnitOfWork(AppDbContext dbContext, IExamRepository examRepository, IAccountRepository accountRepository) : IUnitOfWork
{
    private readonly AppDbContext _context = dbContext;

    public IExamRepository Exam { get; } = examRepository;
    public IAccountRepository Account { get; } = accountRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}