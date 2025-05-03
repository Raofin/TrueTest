﻿using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Constants;
using OPS.Domain.Enums;

namespace OPS.Infrastructure.Auth.Configuration;

internal static class Authorization
{
    /// <summary>
    /// Configures authorization policies based on roles and permissions.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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