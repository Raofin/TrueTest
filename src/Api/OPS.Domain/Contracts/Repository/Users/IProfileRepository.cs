using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository.Users;

public interface IProfileRepository : IBaseRepository<Profile>
{
    Task<Profile?> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
}