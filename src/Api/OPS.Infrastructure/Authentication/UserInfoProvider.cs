using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OPS.Domain.Contracts.Core.Authentication;

namespace OPS.Infrastructure.Authentication;

public class UserInfoProvider(IHttpContextAccessor httpContextAccessor) : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? AccountId()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("accountId")?.Value;
    }

    public string? Username()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("username")?.Value;
    }

    public string? Email()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    public List<string> Roles()
    {
        return _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? [];
    }
}