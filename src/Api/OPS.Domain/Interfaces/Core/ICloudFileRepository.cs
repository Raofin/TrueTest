using OPS.Domain.Entities.Core;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Core;

public interface ICloudFileRepository : IBaseRepository<CloudFile>
{
    Task<bool> IsExistsAsync(Guid cloudFileId, CancellationToken cancellationToken);
}