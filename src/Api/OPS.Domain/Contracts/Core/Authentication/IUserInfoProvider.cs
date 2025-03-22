using OPS.Domain.Enums;

namespace OPS.Domain.Contracts.Core.Authentication;

public interface IUserInfoProvider
{
    bool IsAuthenticated();
    Guid AccountId();
    string Username();
    string Email();
    List<RoleType> Roles();
}