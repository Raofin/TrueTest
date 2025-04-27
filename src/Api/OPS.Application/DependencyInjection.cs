using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Behaviors;
using OPS.Application.Interfaces.Auth;
using OPS.Application.Interfaces.Cloud;
using OPS.Application.Services;

namespace OPS.Application;

[ExcludeFromCodeCoverage]
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

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICloudFileService, CloudFileService>();

        return services;
    }
}