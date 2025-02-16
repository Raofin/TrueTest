using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.BackgroundServices;
using OPS.Application.Behaviors;

namespace OPS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection), includeInternalTypes: true);

        services.AddHostedService<OtpCleanupService>();

        return services;
    }
}