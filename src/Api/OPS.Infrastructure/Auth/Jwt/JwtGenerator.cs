using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OPS.Application.Interfaces.Auth;
using OPS.Domain.Constants;
using OPS.Domain.Entities.User;
using OPS.Domain.Enums;

namespace OPS.Infrastructure.Auth.Jwt;

/// <summary>
/// Implementation of the <see cref="IJwtGenerator"/> interface for creating JSON Web Tokens (JWTs).
/// </summary>
internal class JwtGenerator(IOptions<JwtSettings> jwtSettings) : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    /// <inheritdoc />
    public string CreateToken(Account account)
    {
        var claims = GenerateBaseClaims(account);

        AddRoleClaims(account, claims);
        AddPermissionClaims(account, claims);

        return GenerateJwtToken(claims);
    }

    /// <summary>
    /// Generates the base claims for the JWT, including AccountId, Username, and Email.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity.</param>
    /// <returns>A <see cref="List{Claim}"/> containing the base user claims.</returns>
    private static List<Claim> GenerateBaseClaims(Account account)
    {
        return
        [
            new Claim("AccountId", account.Id.ToString()),
            new Claim("Username", account.Username),
            new Claim("Email", account.Email)
        ];
    }

    /// <summary>
    /// Adds role claims to the JWT based on the user's assigned roles.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity.</param>
    /// <param name="claims">The <see cref="List{Claim}"/> to add role claims to.</param>
    private static void AddRoleClaims(Account account, List<Claim> claims)
    {
        var roleClaims = account.AccountRoles.Select(role =>
            new Claim(ClaimTypes.Role, ((RoleType)role.RoleId).ToString())
        ).ToList();

        claims.AddRange(roleClaims);
    }

    /// <summary>
    /// Adds permission claims to the JWT based on the user's roles.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity.</param>
    /// <param name="claims">The <see cref="List{Claim}"/> to add permission claims to.</param>
    private static void AddPermissionClaims(Account account, List<Claim> claims)
    {
        var allPermissions = GetPermissionsByRoles(account);

        var permissionClaims = allPermissions.Select(permission =>
            new Claim("Permission", permission)
        ).ToList();

        claims.AddRange(permissionClaims);
    }

    /// <summary>
    /// Retrieves all unique permissions for the user based on their assigned roles.
    /// </summary>
    /// <param name="account">The <see cref="Account"/> entity.</param>
    /// <returns>A <see>
    ///         <cref>HashSet{string}</cref>
    ///     </see>
    ///     containing the unique permissions.</returns>
    private static HashSet<string> GetPermissionsByRoles(Account account)
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

    /// <summary>
    /// Generates the final JWT string from the provided claims.
    /// </summary>
    /// <param name="claims">The <see cref="List{Claim}"/> to include in the JWT.</param>
    /// <returns>A <see cref="string"/> representing the generated JWT.</returns>
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