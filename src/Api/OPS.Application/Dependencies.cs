using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Contracts;
using OPS.Application.Implementation;

namespace OPS.Application;

public static class Dependencies
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IExamService, ExamService>();

        return services;
    }
}
