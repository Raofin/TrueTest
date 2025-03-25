using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository.Users;

public interface IProfileLinkRepository : IBaseRepository<ProfileLinks>
{
    Task<List<ProfileLinks>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
}