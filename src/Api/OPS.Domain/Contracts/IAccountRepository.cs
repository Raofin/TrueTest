using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Contracts;

public interface IAccountRepository : IRepository<Account>
{
    Task<bool> IsUsernameOrEmailTakenAsync(string username, string email, CancellationToken cancellationToken);
    Task<bool> IsExistsAsync(string? username, string? email, CancellationToken cancellationToken);
    Task<Account?> GetWithDetails(string usernameOrEmail, CancellationToken cancellationToken);
}