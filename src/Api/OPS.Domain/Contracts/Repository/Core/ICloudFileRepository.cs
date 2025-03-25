using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.Core;

namespace OPS.Domain.Contracts.Repository.Core;

public interface ICloudFileRepository : IBaseRepository<CloudFile>
{
    Task<bool> IsExistsAsync(Guid cloudFileId, CancellationToken cancellationToken);
}