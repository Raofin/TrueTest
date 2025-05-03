﻿using OPS.Domain.Entities.Common;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Users;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<bool> IsUsernameOrEmailUniqueAsync(string? username, string? email, CancellationToken cancellationToken);
    Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<List<Account>> GetByEmailsAsync(List<string> emails, CancellationToken cancellationToken);
    Task<List<Account>> GetByEmailsWithRoleAsync(List<string> emails, CancellationToken cancellationToken);
    Task<PaginatedList<Account>> GetAllWithDetails(int pageIndex, int pageSize,
        string? searchTerm = null, RoleType? role = null, CancellationToken cancellationToken = default);
    Task<Account?> GetWithDetails(string usernameOrEmail, CancellationToken cancellationToken);
    Task<Account?> GetWithDetails(Guid accountId, CancellationToken cancellationToken);
    Task<List<Account>> GetNonAdminAccounts(List<Guid> accountIds, CancellationToken cancellationToken);
}