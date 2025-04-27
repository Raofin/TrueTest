using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Users;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
}