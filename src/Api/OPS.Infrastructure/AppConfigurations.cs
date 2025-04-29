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
            .AddOneCompilerServices(configuration)
            .AddMemoryCache()
            .AddGoogleCloudServices(configuration)
            .AddGeminiServices(configuration)
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