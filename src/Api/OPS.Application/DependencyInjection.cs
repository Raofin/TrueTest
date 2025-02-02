using Microsoft.Extensions.DependencyInjection;

namespace OPS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
        });
        
        return services;
    }
}
