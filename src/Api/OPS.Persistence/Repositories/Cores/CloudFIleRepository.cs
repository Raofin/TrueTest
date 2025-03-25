using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository.Core;
using OPS.Domain.Entities.Core;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Cores;

internal class CloudFIleRepository(AppDbContext dbContext) : Repository<CloudFile>(dbContext), ICloudFileRepository
{
    private readonly AppDbContext _dbContext = dbContext;
    
    public async Task<bool> IsExistsAsync(Guid cloudFileId, CancellationToken cancellationToken)
    {
        return await _dbContext.CloudFiles
            .AsNoTracking()
            .Where(cf => cf.Id == cloudFileId)
            .AnyAsync(cancellationToken);
    }
}