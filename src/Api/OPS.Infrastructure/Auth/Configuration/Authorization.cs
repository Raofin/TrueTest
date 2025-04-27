using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constents;
using OPS.Domain.Enums;

namespace OPS.Infrastructure.Auth.Configuration;

internal static class Authorization
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        var builder = services.AddAuthorizationBuilder();

        foreach (var role in Enum.GetValues<RoleType>())
        {
            var roleName = Enum.GetName(role)!;

            builder.AddPolicy(roleName, policy => policy.RequireRole(roleName));
        }

        var permissions = typeof(Permissions)
            .GetFields()
            .Select(f => f.GetValue(null)?.ToString())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        foreach (var permission in permissions)
        {
            builder.AddPolicy(permission!, policy => policy.RequireClaim("Permission", permission!));
        }

        return services;
    }
}