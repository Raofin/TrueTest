using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces;
using OPS.Infrastructure.OneCompiler.Refit;
using Refit;

namespace OPS.Infrastructure.OneCompiler.Configuration;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add OneCompiler related services.
/// </summary>
internal static class OneCompiler
{
    /// <summary>
    /// Adds OneCompiler services to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="config">The <see cref="IConfiguration"/> containing application settings, including OneCompiler configuration.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    internal static IServiceCollection AddOneCompilerServices(this IServiceCollection services, IConfiguration config)
    {
        var oneCompilerSettings = new OneCompilerSettings();
        config.Bind(nameof(OneCompilerSettings), oneCompilerSettings);

        services.AddSingleton(oneCompilerSettings);

        services.AddRefitClient<IOneCompilerClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(oneCompilerSettings.BaseUrl);
                client.DefaultRequestHeaders.Add("X-Rapidapi-Key", oneCompilerSettings.ApiKey);
                client.DefaultRequestHeaders.Add("X-Rapidapi-Host", oneCompilerSettings.ApiHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

        services.AddScoped<IOneCompilerService, OneCompilerService>();

        return services;
    }
}