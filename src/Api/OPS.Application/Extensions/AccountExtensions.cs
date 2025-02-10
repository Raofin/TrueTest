using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using OPS.Application.Contracts.Auth;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Entities.Core;

namespace OPS.Application.Extensions;

public static class AccountExtensions
{
    public static AccountResponse ToDto(this Account account)
    {
        return new AccountResponse(
            account.AccountId,
            account.Username,
            account.Email,
            account.PasswordHash,
            account.Salt,
            account.IsVerified,
            account.CloudFileId,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive,
            account.IsDeleted
        );
    }
}