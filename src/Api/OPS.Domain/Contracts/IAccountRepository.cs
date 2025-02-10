using OPS.Domain.Entities.Auth;

namespace OPS.Domain.Contracts;

public interface IAccountRepository : IRepository<Account>
{
    Task<List<Account>> GetUpcomingAccountAsync(CancellationToken cancellationToken);
}