using OPS.Application.Contracts.Dtos;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Contracts.DtoExtensions;

public static class AccountExtensions
{
    public static AccountResponse ToDto(this Account account)
    {
        var roles = account.AccountRoles
            .Select(accountRole => (RoleType)accountRole.RoleId)
            .ToList();
        
        return new AccountResponse(
            account.Id,
            account.Username,
            account.Email,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive,
            roles
        );
    }
}
