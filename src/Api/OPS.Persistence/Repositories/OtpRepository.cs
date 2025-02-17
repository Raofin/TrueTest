using Microsoft.EntityFrameworkCore;
using OPS.Domain.Contracts.Repository;
using OPS.Domain.Entities.User;

namespace OPS.Persistence.Repositories;

internal class OtpRepository(AppDbContext dbContext) : Repository<Otp>(dbContext), IOtpRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> IsValidOtpAsync(string email, string otp, CancellationToken cancellationToken)
    {
        return await _dbContext.Otps
            .AsNoTracking()
            .Where(o => o.Email == email && o.Code == otp && o.ExpiresAt > DateTime.UtcNow)
            .AnyAsync(cancellationToken);
    }

    public async Task<Otp?> GetOtpAsync(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Otps
            .AsNoTracking()
            .Where(o => o.Email == email)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Otp?> GetValidOtpAsync(string email, string otp, CancellationToken cancellationToken)
    {
        return await _dbContext.Otps
            .AsNoTracking()
            .Where(o => o.Email == email && o.Code == otp && o.ExpiresAt > DateTime.UtcNow)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Otp>> GetExpiredOtpsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Otps
            .AsNoTracking()
            .Where(o => o.ExpiresAt < DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }
}