using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Enums;

namespace OPS.Infrastructure.Authentication;

public class UserInfoProvider(IHttpContextAccessor httpContextAccessor) : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity?.IsAuthenticated ?? false;
    }

    public Guid AccountId()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.User.FindFirst("accountId")?.Value;

        return Guid.TryParse(accountIdStr, out var accountId)
            ? accountId
            : throw new UnauthorizedAccessException("Account ID is missing or invalid.");
    }

    public string Username()
    {
        var username = _httpContextAccessor.HttpContext?.User.FindFirst("username")?.Value;

        return !string.IsNullOrEmpty(username) 
            ? username 
            : throw new UnauthorizedAccessException("Username is missing.");
    }

    public string Email()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

        return !string.IsNullOrEmpty(email) 
            ? email 
            : throw new UnauthorizedAccessException("Email is missing.");
    }

    public List<RoleType> Roles()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindAll(ClaimTypes.Role)
            .Select(x => Enum.Parse<RoleType>(x.Value))
            .ToList() ?? [];
    }
}