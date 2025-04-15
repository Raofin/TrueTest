using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Infrastructure.Authentication;

namespace OPS.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IOtpGenerator, OtpGenerator>();
        services.AddScoped<IUserInfoProvider, UserInfoProvider>();

        return services;
    }
}