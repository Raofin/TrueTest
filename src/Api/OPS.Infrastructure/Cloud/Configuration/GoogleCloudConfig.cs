using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces.Cloud;

namespace OPS.Infrastructure.Cloud.Configuration;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add Google Cloud related services.
/// </summary>
public static class GoogleCloudConfig
{
    /// <summary>
    /// Adds Google Cloud services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings, including Google Cloud configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddGoogleCloudServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new GoogleCloudSettings();
        configuration.Bind(nameof(GoogleCloudSettings), settings);

        services.AddSingleton(_ => new DriveServiceProvider(settings).GetDriveService());
        services.AddScoped<IGoogleCloudService, GoogleCloudService>();

        return services;
    }
}