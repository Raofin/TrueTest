using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OPS.Application.Interfaces.Auth;
using OPS.Infrastructure.Auth.Jwt;

namespace OPS.Infrastructure.Auth.Configuration;

internal static class Authentication
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

        services.AddSingleton<IJwtGenerator, JwtGenerator>();

        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
            throw new InvalidOperationException("JWT settings are not configured properly.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtSettings.Secret)
                    )
                };
            });

        return services;
    }
}