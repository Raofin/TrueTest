using OPS.Domain.Contracts.Repository.Common;
using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository.Users;

public interface IAdminInviteRepository : IBaseRepository<AdminInvite>
{
    Task<bool> IsExistsAsync(string email, CancellationToken cancellationToken);
    Task<List<string>> GetUninvitedEmails(List<string> emails, CancellationToken cancellationToken);
}