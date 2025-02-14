using Microsoft.Extensions.DependencyInjection;

namespace OPS.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        return services;
    }
}