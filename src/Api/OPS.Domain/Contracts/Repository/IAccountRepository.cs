using OPS.Domain.Entities.User;

namespace OPS.Domain.Contracts.Repository;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<bool> IsUsernameOrEmailUniqueAsync(string? username, string? email, CancellationToken cancellationToken);
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> IsExistsAsync(string? username, string? email, CancellationToken cancellationToken);
    Task<Account?> GetWithDetails(string usernameOrEmail, CancellationToken cancellationToken);
}