using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Users;

public interface IAdminInviteRepository : IBaseRepository<AdminInvite>
{
    Task<bool> IsExistsAsync(string email, CancellationToken cancellationToken);
    Task<AdminInvite?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<List<string>> GetUninvitedEmails(List<string> emails, CancellationToken cancellationToken);
}