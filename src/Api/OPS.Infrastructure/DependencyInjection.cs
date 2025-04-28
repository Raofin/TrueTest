using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Services;
using OPS.Infrastructure.Auth;
using OPS.Infrastructure.Auth.Permission;
using OPS.Infrastructure.BackgroundServices;

namespace OPS.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IOtpGenerator, OtpGenerator>();

        services.AddScoped<IUserProvider, UserProvider>();

        services.AddHostedService<OtpCleanupService>();

        return services;
    }
}