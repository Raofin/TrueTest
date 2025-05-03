using Microsoft.EntityFrameworkCore;
using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Users;
using OPS.Persistence.Repositories.Common;

namespace OPS.Persistence.Repositories.Users;

internal class AdminInviteRepository(AppDbContext dbContext)
    : Repository<AdminInvite>(dbContext), IAdminInviteRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> IsExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.AdminInvites
            .AsNoTracking()
            .Where(a => a.Email == email)
            .AnyAsync(cancellationToken);
    }

    public async Task<AdminInvite?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.AdminInvites
            .AsNoTracking()
            .Where(a => a.Email == email)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<string>> GetUninvitedEmails(List<string> emails, CancellationToken cancellationToken)
    {
        var invitedEmails = await _dbContext.AdminInvites
            .AsNoTracking()
            .Where(ai => emails.Contains(ai.Email))
            .Select(ai => ai.Email)
            .ToListAsync(cancellationToken);

        return emails.Except(invitedEmails).ToList();
    }
}