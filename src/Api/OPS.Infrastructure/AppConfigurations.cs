using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OPS.Infrastructure.Auth.Configuration;
using OPS.Infrastructure.Cloud.Configuration;
using OPS.Infrastructure.Database;
using OPS.Infrastructure.Email.Configuration;
using OPS.Infrastructure.Gemini.Configuration;
using OPS.Infrastructure.Logging;
using OPS.Infrastructure.OneCompiler.Configuration;
using Serilog;

namespace OPS.Infrastructure;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> and <see cref="IApplicationBuilder"/> to configure infrastructure services.
/// </summary>
public static class AppConfigurations
{
    /// <summary>
    /// Adds various infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings.</param>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> for configuring the host.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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
            .AddOneCompilerServices(configuration)
            .AddMemoryCache()
            .AddGoogleCloudServices(configuration)
            .AddGeminiServices(configuration)
            .AddDependencies()
            .AddHealthChecks();

        hostBuilder.AddSerilog(configuration);

        return services;
    }

    /// <summary>
    /// Configures the application to use infrastructure-related middleware.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }
}