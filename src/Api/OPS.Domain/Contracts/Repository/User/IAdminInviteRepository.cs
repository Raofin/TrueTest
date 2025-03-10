using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository;

public interface IAdminInviteRepository : IBaseRepository<AdminInvite>
{
    Task<bool> IsExistsAsync(string email, CancellationToken cancellationToken);
}