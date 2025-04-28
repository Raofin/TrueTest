using OPS.Application.Dtos;

namespace OPS.Application.Interfaces.Auth;

public interface IUserProvider
{
    bool IsAuthenticated();
    CurrentUser GetCurrentUser();
    List<string> GetPermissions();
    Guid AccountId();
    Guid? TryGetAccountId();
    dynamic DecodeToken();
}