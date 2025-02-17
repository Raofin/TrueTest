using OPS.Domain.Enums;

namespace OPS.Domain.Contracts.Core.Authentication;

public interface IUserInfoProvider
{
    string? AccountId();
    string? Username();
    string? Email();
    List<RoleType> Roles();
}