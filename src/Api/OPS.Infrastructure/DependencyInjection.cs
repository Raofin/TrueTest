using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Contracts.Core.GoogleCloud;
using OPS.Infrastructure.Authentication.Otp;
using OPS.Infrastructure.Authentication.Permission;
using OPS.Infrastructure.Authentication.User;
using OPS.Infrastructure.GoogleCloud;

namespace OPS.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddSingleton<IOtpGenerator, OtpGenerator>();
        services.AddScoped<IUserInfoProvider, CurrentUserProvider>();

        services.AddScoped<IGoogleCloudService, GoogleCloudService>();

        return services;
    }
}