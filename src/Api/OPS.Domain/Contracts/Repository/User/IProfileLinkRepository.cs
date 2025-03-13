using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts;

public interface IProfileLinkRepository : IBaseRepository<ProfileLinks>
{
    Task<List<ProfileLinks>> GetByProfileIdAsync(Guid profileId, CancellationToken cancellationToken);
}