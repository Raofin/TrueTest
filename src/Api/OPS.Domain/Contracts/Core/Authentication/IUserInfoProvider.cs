namespace OPS.Domain.Contracts.Core.Authentication;

public interface IUserInfoProvider
{
    bool IsAuthenticated();
    CurrentUser GetCurrentUser();
    List<string> GetPermissions();
    Guid AccountId();
    Guid? TryGetAccountId();
    dynamic DecodeToken();
}