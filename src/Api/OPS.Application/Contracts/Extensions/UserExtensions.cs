using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.Auth;

namespace OPS.Application.Contracts.Extensions;

public static class AccountExtensions
{
    public static AccountResponse ToDto(this Account account)
    {
        return new AccountResponse(
            account.Id,
            account.Username,
            account.Email,
            account.IsVerified,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive
        );
    }
}