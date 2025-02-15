using Microsoft.Extensions.DependencyInjection;
using OPS.Domain;
using OPS.Domain.Contracts.Repository;
using OPS.Persistence.Repositories;

namespace OPS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<IExamRepository, ExamRepository>();

        return services;
    }
}