using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OPS.Application.Dtos;
using OPS.Application.Interfaces.Auth;
using OPS.Domain.Enums;
using Throw;

namespace OPS.Application.Services;

public class UserProvider(IHttpContextAccessor httpContextAccessor) : IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity?.IsAuthenticated ?? false;
    }

    public CurrentUser GetCurrentUser()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        var user = _httpContextAccessor.HttpContext.User;
        var accountId = user.Claims.First(c => c.Type == "AccountId").Value;
        var username = user.Claims.First(c => c.Type == "Username").Value;
        var email = user.Claims.First(c => c.Type == "Email").Value;

        var permissions = user.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToList();

        var roles = user.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(role => Enum.Parse<RoleType>(role.Value))
            .ToList();

        return new CurrentUser(
            Guid.Parse(accountId),
            username,
            email,
            permissions,
            roles
        );
    }

    public List<string> GetPermissions()
    {
        _httpContextAccessor.HttpContext.ThrowIfNull();

        return _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToList();
    }

    public Guid AccountId()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.User.FindFirst("AccountId")?.Value;

        return Guid.TryParse(accountIdStr, out var accountId)
            ? accountId
            : throw new UnauthorizedAccessException("Account ID is missing or invalid.");
    }

    public Guid? TryGetAccountId()
    {
        var accountIdStr = _httpContextAccessor.HttpContext?.User.FindFirst("AccountId")?.Value;
        return Guid.TryParse(accountIdStr, out var id) ? id : null;
    }

    [ExcludeFromCodeCoverage]
    public dynamic DecodeToken()
    {
        var claims = _httpContextAccessor.HttpContext?.User.Claims
            .Select(c => new { c.Type, c.Value })
            .ToList();

        var result = new Dictionary<string, object>();

        foreach (var claim in claims!)
        {
            if (result.ContainsKey(claim.Type))
            {
                if (result[claim.Type] is List<string> list)
                {
                    list.Add(claim.Value);
                }
                else
                {
                    result[claim.Type] = new List<string?>
                    {
                        result[claim.Type].ToString(),
                        claim.Value
                    };
                }
            }
            else
            {
                result[claim.Type] = claim.Value;
            }
        }

        return result;
    }
}