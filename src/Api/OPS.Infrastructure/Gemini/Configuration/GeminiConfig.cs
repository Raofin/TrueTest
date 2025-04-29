using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OPS.Infrastructure.Gemini.Refit;
using Refit;

namespace OPS.Infrastructure.Gemini.Configuration;

public static class GeminiConfig
{
    internal static IServiceCollection AddGeminiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeminiSettings>(configuration.GetSection(nameof(GeminiSettings)));

        services.AddRefitClient<IGeminiClient>()
            .ConfigureHttpClient((serviceProvider, c) =>
                {
                    var settings = serviceProvider.GetRequiredService<IOptions<GeminiSettings>>().Value;
                    c.BaseAddress = new Uri(settings.BaseUrl);
                }
            );

        return services;
    }
}