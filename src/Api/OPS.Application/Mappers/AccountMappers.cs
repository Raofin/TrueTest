using OPS.Application.Dtos;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Application.Mappers;

public static class AccountExtensions
{
    public static AccountResponse ToDto(this Account account)
    {
        return new AccountResponse(
            account.Id,
            account.Username,
            account.Email,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive
        );
    }

    public static AccountWithDetailsResponse ToDtoWithDetails(this Account account)
    {
        var roles = account.AccountRoles
            .Select(accountRole => (RoleType)accountRole.RoleId)
            .ToList();

        return new AccountWithDetailsResponse(
            account.Id,
            account.Username,
            account.Email,
            account.CreatedAt,
            account.UpdatedAt,
            account.IsActive,
            roles,
            account.Profile.ToDto()
        );
    }

    public static ProfileResponse? ToDto(this Profile? profile)
    {
        return profile is null
            ? null
            : new ProfileResponse(
                profile.Id,
                profile.FirstName,
                profile.LastName,
                profile.Bio,
                profile.InstituteName,
                profile.PhoneNumber,
                profile.ImageFileId,
                profile.ProfileLinks
                    .Select(pl => new ProfileLinkRequest(pl.Id, pl.Name, pl.Link)).ToList()
            );
    }
}