using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Auth;

namespace OPS.Persistence.Repositories;

internal class AccountRepository(AppDbContext dbContext) : Repository<Account>(dbContext), IAccountRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Account>> GetUpcomingAccountAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}