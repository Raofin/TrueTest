using Microsoft.Extensions.DependencyInjection;
using OPS.Domain;

namespace OPS.Persistence;

public static class Dependencies
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

}
