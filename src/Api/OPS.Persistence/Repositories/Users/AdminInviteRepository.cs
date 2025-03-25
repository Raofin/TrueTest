using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Users;
using OPS.Domain.Entities.User;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Users;

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