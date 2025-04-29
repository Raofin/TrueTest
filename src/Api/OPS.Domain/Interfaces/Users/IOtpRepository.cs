using OPS.Domain.Entities.User;
using OPS.Domain.Interfaces.Common;

namespace OPS.Domain.Interfaces.Users;

public interface IOtpRepository : IBaseRepository<Otp>
{
    Task<bool> IsValidOtpAsync(string email, string otp, CancellationToken cancellationToken);
    Task<Otp?> GetOtpAsync(string email, CancellationToken cancellationToken);
    Task<Otp?> GetValidOtpAsync(string email, string otp, CancellationToken cancellationToken);
    Task<List<Otp>> GetExpiredOtpsAsync(CancellationToken cancellationToken);
}