using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;
using OPS.Infrastructure.AppConfiguration.Auth;
using OPS.Infrastructure.Authentication.Permission;

namespace OPS.Infrastructure.Authentication.Jwt;

internal class JwtGenerator(IOptions<JwtSettings> jwtSettings) : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string CreateToken(Account account)
    {
        var claims = GenerateBaseClaims(account);

        AddRoleClaims(account, claims);
        AddPermissionClaims(account, claims);

        return GenerateJwtToken(claims);
    }

    private List<Claim> GenerateBaseClaims(Account account)
    {
        return
        [
            new Claim("AccountId", account.Id.ToString()),
            new Claim("Username", account.Username),
            new Claim("Email", account.Email)
        ];
    }

    private void AddRoleClaims(Account account, List<Claim> claims)
    {
        var roleClaims = account.AccountRoles.Select(role =>
            new Claim(ClaimTypes.Role, ((RoleType)role.RoleId).ToString())
        ).ToList();

        claims.AddRange(roleClaims);
    }

    private void AddPermissionClaims(Account account, List<Claim> claims)
    {
        var allPermissions = GetPermissionsByRoles(account);

        var permissionClaims = allPermissions.Select(permission =>
            new Claim("Permission", permission)
        ).ToList();

        claims.AddRange(permissionClaims);
    }

    private HashSet<string> GetPermissionsByRoles(Account account)
    {
        var allPermissions = new HashSet<string>();

        foreach (var role in account.AccountRoles)
        {
            var roleType = (RoleType)role.RoleId;
            var permissions = Permissions.ByRole[roleType];

            foreach (var permission in permissions)
            {
                allPermissions.Add(permission);
            }
        }

        return allPermissions;
    }

    private string GenerateJwtToken(List<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                signingCredentials: credentials
            )
        );

        return token;
    }
}