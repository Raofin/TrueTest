using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class AccountRepository(AppDbContext dbContext) : Repository<Account>(dbContext), IAccountRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> IsUsernameOrEmailUniqueAsync(
        string? username, string? email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email))
            throw new ArgumentException();

        var exists = await _dbContext.Accounts
            .AsNoTracking()
            .Where(a =>
                (!string.IsNullOrEmpty(username) && a.Username == username) ||
                (!string.IsNullOrEmpty(email) && a.Email == email))
            .AnyAsync(cancellationToken);

        return !exists;
    }

    public async Task<bool> IsExistsAsync(string? username, string? email, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .Where(a =>
                (!string.IsNullOrEmpty(username) && a.Username == username) ||
                (!string.IsNullOrEmpty(email) && a.Email == email))
            .AnyAsync(cancellationToken);
    }

    public async Task<Account?> GetWithDetails(string usernameOrEmail, CancellationToken cancellationToken)
    {
        return await _dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.AccountRoles)
            .ThenInclude(ar => ar.Role)
            .Include(a => a.Profile)
            .Where(a => a.Username == usernameOrEmail || a.Email == usernameOrEmail)
            .SingleOrDefaultAsync(cancellationToken);
    }
}