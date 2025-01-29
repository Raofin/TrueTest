using OPS.Domain;
using OPS.Domain.Interfaces.Repositories;

namespace OPS.Persistence;

internal class UnitOfWork(AppDbContext dbContext, IExamRepository examRepository) : IUnitOfWork
{
    private readonly AppDbContext _context = dbContext;

    public IExamRepository Exam { get; } = examRepository;

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}