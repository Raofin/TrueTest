using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Entities.Auth;
using OPS.Domain.Enums;

namespace OPS.Infrastructure.Authentication;

internal class JwtGenerator(IOptions<JwtSettings> jwtSettings) : IJwtGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public string CreateToken(Account account)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var claims = new List<Claim>
        {
            new("accountId", account.Id.ToString()),
            new("username", account.Username),
            new(JwtRegisteredClaimNames.Email, account.Email)
        };

        account.AccountRoles.ToList().ForEach(
            role => claims.Add(new Claim(
                ClaimTypes.Role,
                ((RoleType)role.RoleId).ToString())
            )
        );

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