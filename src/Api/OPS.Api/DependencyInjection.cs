using System.Reflection;
using Microsoft.OpenApi.Models;
using OPS.Api.Middlewares;
using Scalar.AspNetCore;
using OPS.Api.Transformers;
using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddCorsWithOrigins(configuration);

        services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
        services.AddSwagger();

        return services;
    }

    private static void AddCorsWithOrigins(this IServiceCollection services, IConfiguration configuration)
    {
        var corsConfig = configuration.GetSection("Cors");

        if (corsConfig == null)
            throw new InvalidOperationException("CORS configuration section is missing in appsettings.json");

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                var allowAnyOrigin = corsConfig.GetValue<bool>("AllowAnyOrigin");

                if (allowAnyOrigin)
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
                else
                {
                    var allowedOrigins = corsConfig.GetSection("AllowedOrigins").Get<string[]>();

                    policy.WithOrigins(allowedOrigins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            });
        });
    }

    public static void UseControllers(this WebApplication app)
    {
        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseHealthChecks("/health");
        app.UseCors("CorsPolicy");

        if (app.Environment.IsProduction())
            app.UseMiddleware<ExceptionHandleMiddleware>();
    }

    public static void UseScalar(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Online Proctoring System")
                // .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
                .WithDefaultOpenAllTags(true)
                /*.WithLayout(ScalarLayout.Classic)*/;
        });
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TrueTest - API Documentation",
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

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<Program>();
    }
}