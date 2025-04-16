using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OPS.Infrastructure.AppConfiguration.Auth;
using OPS.Infrastructure.AppConfiguration.Database;
using OPS.Infrastructure.AppConfiguration.Email;
using OPS.Infrastructure.AppConfiguration.Logging;
using Serilog;

namespace OPS.Infrastructure.AppConfiguration;

public static class AppConfigurations
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostBuilder hostBuilder)
    {
        services
            .AddDatabaseServices(configuration)
            .AddAuthorizationServices()
            .AddAuthenticationServices(configuration)
            .AddEmailServices(configuration)
            .AddMemoryCache()
            .AddDependencies()
            .AddHealthChecks();

        hostBuilder.AddSerilog(configuration);

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}