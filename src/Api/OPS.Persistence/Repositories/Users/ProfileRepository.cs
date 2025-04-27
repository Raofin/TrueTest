using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Users;
using OPS.Domain.Entities.User;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Users;

internal class ProfileRepository(AppDbContext dbContext) : Repository<Profile>(dbContext), IProfileRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Profile?> GetByAccountId(Guid accountId, CancellationToken cancellationToken)
    {
        return await _dbContext.Profiles
            .Where(p => p.AccountId == accountId)
            .Include(p => p.ProfileLinks)
            .Include(p => p.ImageFile)
            .SingleOrDefaultAsync(cancellationToken);
    }
}