using Microsoft.Extensions.DependencyInjection;
using OPS.Service.Contract;
using OPS.Service.Implementation;

namespace OPS.Service;

public static class Dependencies
{
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddScoped<IExamService, ExamService>();

        return services;
    }

}
