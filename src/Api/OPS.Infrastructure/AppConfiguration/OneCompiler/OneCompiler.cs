using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Domain.Contracts.Core.OneCompiler;
using OPS.Infrastructure.OneCompiler;
using OPS.Infrastructure.OneCompiler.Refit;
using Refit;

namespace OPS.Infrastructure.AppConfiguration.OneCompiler;

internal static class OneCompiler
{
    internal static IServiceCollection AddOneCompilerServices(this IServiceCollection services, IConfiguration config)
    {
        var oneCompilerSettings = new OneCompilerSettings();
        config.Bind(nameof(OneCompilerSettings), oneCompilerSettings);

        services.AddSingleton(oneCompilerSettings);

        services.AddRefitClient<IOneCompilerApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(oneCompilerSettings.BaseUrl);
                client.DefaultRequestHeaders.Add("X-Rapidapi-Key", oneCompilerSettings.ApiKey);
                client.DefaultRequestHeaders.Add("X-Rapidapi-Host", oneCompilerSettings.ApiHost);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

        services.AddScoped<IOneCompilerApiService, OneCompilerApiService>();

        return services;
    }
}