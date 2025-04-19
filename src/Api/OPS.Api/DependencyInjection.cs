using System.Reflection;
using Microsoft.OpenApi.Models;
using OPS.Api.Middlewares;
using Scalar.AspNetCore;
using OPS.Api.Transformers;
using OPS.Application.Common.Constants;
using Swashbuckle.AspNetCore.Filters;

namespace OPS.Api;

internal static class DependencyInjection
{
    public static void UseControllers(this WebApplication app)
    {
        app.UseMiddleware<GlobalRoutePrefixMiddleware>("/api");
        app.UsePathBase(new PathString("/api"));
        app.UseHttpsRedirection();

        app.UseHealthChecks("/health");
        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseStaticFiles();

        if (app.Environment.IsProduction())
            app.UseMiddleware<ExceptionHandleMiddleware>();
    }

    public static void UseApiDocumentation(this WebApplication app)
    {
        app.UseScalar();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = $"{ProjectConstants.ProjectName} - Swagger";
            c.DefaultModelsExpandDepth(0);
            c.DisplayRequestDuration();
            c.InjectStylesheet("/swagger/custom.css");
            c.InjectJavascript("/swagger/custom.js");
        });

        app.MapGet("/", context =>
        {
            context.Response.Redirect("swagger");
            return Task.CompletedTask;
        });
    }

    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddProblemDetails();
        services.AddCorsWithOrigins(configuration);

        services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
        services.AddSwagger();
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

    private static void UseScalar(this WebApplication app)
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

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description =
                        "🗝️ Admin Account\n\neyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJBY2NvdW50SWQiOiIxMTExMTExMS0xMTExLTExMTEtMTExMS0xMTExMTExMTExMTEiLCJVc2VybmFtZSI6ImFkbWluIiwiRW1haWwiOiJhZG1pbkB0cnVldGVzdC50ZWNoIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjpbIkNhbmRpZGF0ZSIsIkFkbWluIl0sIlBlcm1pc3Npb24iOlsiQWNjZXNzT3duRXhhbXMiLCJTdWJtaXRBbnN3ZXJzIiwiUnVuQ29kZSIsIk1hbmFnZU93blByb2ZpbGUiLCJNYW5hZ2VBY2NvdW50cyIsIlZpZXdFeGFtcyIsIk1hbmFnZUV4YW1zIiwiTWFuYWdlUXVlc3Rpb25zIiwiVmlld1N1Ym1pc3Npb25zIiwiUmV2aWV3U3VibWlzc2lvbiJdLCJleHAiOjQ4OTkwMzMxOTUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0In0.VBPngQIwsybHrwZPYMA5sNpWew3S9_W7cDdpAD4mDyM"
                }
            );

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