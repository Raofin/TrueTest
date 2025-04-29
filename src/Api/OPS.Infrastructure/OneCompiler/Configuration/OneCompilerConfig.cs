using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Application.Interfaces;
using OPS.Infrastructure.OneCompiler.Refit;
using Refit;

namespace OPS.Infrastructure.OneCompiler.Configuration;

internal static class OneCompiler
{
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