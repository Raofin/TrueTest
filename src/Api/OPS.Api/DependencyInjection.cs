using Scalar.AspNetCore;
using OPS.Api.Transformers;

namespace OPS.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddController(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

        return services;
    }

    public static void UseControllers(this WebApplication app)
    {
        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseHealthChecks("/health");
    }

    public static void UseScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Online Proctoring System")
                .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
                .WithDefaultOpenAllTags(true)
                .WithLayout(ScalarLayout.Classic);
        });
    }
}