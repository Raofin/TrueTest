using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class AdminInviteRepository(AppDbContext dbContext) : Repository<AdminInvite>(dbContext), IAdminInviteRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<bool> IsExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.AdminInvites
            .AsNoTracking()
            .Where(a => a.Email == email)
            .AnyAsync(cancellationToken);
    }
}