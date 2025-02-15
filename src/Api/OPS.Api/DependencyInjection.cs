using Microsoft.OpenApi.Models;
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
        services.AddSwagger();

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
                /*.WithLayout(ScalarLayout.Classic)*/;
        });
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Online Proctoring System",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }
}