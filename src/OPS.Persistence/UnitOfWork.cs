using OPS.Domain;

namespace OPS.Persistence;

internal class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _context = dbContext;

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}