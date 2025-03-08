using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class ProfileLinkRepository(AppDbContext dbContext) : Repository<ProfileLinks>(dbContext), IProfileLinkRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ProfileLinks>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProfileLinks
            .Where(q => q.ProfileId == profileId)
            .ToListAsync(cancellationToken);
    }
}