using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts;
using OPS.Domain.Entities.Exam;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class ProfileSocialRepository(AppDbContext dbContext) : Repository<ProfileSocial>(dbContext), IProfileSocialRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ProfileSocial>> GetProfileSocialsByProfileIdAsync(Guid profileId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProfileSocials
            .AsNoTracking()
            .Where(q => q.ProfileId == profileId)
            .ToListAsync(cancellationToken);
    }
}