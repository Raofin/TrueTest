using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces.Cloud;

namespace OPS.Infrastructure.Cloud.Configuration;

public static class GoogleCloudConfig
{
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