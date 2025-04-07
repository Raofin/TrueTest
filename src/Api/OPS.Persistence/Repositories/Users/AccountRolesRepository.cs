using OPS.Domain.Contracts.Repository.Users;
using OPS.Domain.Entities.User;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Users;

internal class AccountRoleRepository(AppDbContext dbContext)
    : Repository<AccountRole>(dbContext), IAccountRoleRepository
{
}