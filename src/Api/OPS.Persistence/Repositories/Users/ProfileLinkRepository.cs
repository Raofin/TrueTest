using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Users;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Users;

internal class ProfileLinkRepository(AppDbContext dbContext)
    : Repository<ProfileLinks>(dbContext), IProfileLinkRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ProfileLinks>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProfileLinks
            .Where(q => q.ProfileId == profileId)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}