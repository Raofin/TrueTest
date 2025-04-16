using OPS.Domain.Enums;

namespace OPS.Domain.Contracts.Core.Authentication;

public record CurrentUser(
    Guid AccountId,
    string Username,
    string Email,
    List<string> Permissions,
    List<RoleType> Roles
);