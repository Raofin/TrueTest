using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Users;

public interface IProfileLinkRepository : IBaseRepository<ProfileLinks>
{
    Task<List<ProfileLinks>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
}