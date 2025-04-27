using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OPS.Infrastructure.AppConfiguration.GoogleCloud;

public static class CloudConfiguration
{
    public static IServiceCollection AddGoogleCloudServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new GoogleCloudSettings();
        configuration.Bind(nameof(GoogleCloudSettings), settings);

        services.AddSingleton(_ => new DriveServiceProvider(settings).GetDriveService());

        return services;
    }
}