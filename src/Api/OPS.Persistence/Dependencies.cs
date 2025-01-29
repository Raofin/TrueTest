using Microsoft.Extensions.DependencyInjection;
using OPS.Domain;
using OPS.Domain.Interfaces.Repositories;
using OPS.Persistence.Repositories;

namespace OPS.Persistence;

public static class Dependencies
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IExamRepository, ExamRepository>();

        return services;
    }

}
