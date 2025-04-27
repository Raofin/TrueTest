using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace OPS.Infrastructure.AppConfiguration.GoogleCloud;

public static class CloudConfiguration
{
    public static IServiceCollection AddGoogleCloudServices(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new GoogleCloudSettings();
        configuration.Bind(nameof(GoogleCloudSettings), settings);

        services.AddSingleton(serviceProvider => {
            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            var logger = serviceProvider.GetRequiredService<ILogger>();

            return new DriveServiceProvider(memoryCache, settings, logger).GetDriveService();
        });

        return services;
    }
}