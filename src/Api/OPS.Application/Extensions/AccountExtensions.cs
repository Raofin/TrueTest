using OPS.Application.Contracts.Auth;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Extensions;

public static class AccountExtensions
{
    public static AccountResponse ToDto(this Account account)
    {
        return new AccountResponse(
            account.Id,
            account.Username,
            account.Email,
            account.PasswordHash,
            account.Salt,
            account.IsVerified,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive,
            account.IsDeleted
        );
    }
}