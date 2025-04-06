using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OPS.Application.Common.Constants;
using OPS.Domain.Contracts.Core.Authentication;
using OPS.Domain.Contracts.Core.EmailSender;
using OPS.Domain.Enums;
using OPS.Infrastructure.Authentication.Jwt;
using OPS.Infrastructure.Authentication.Password;
using OPS.Infrastructure.Authentication.Permission;
using OPS.Infrastructure.EmailSender;
using OPS.Persistence;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace OPS.Infrastructure;

public static class AppConfigurations
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        IHostBuilder hostBuilder)
    {
        services
            .AddDatabase(configuration)
            .AddAuthentication(configuration)
            .AddAuthorization()
            .AddEmailSettings(configuration)
            .AddMemoryCache()
            .AddDependencies()
            .AddHealthChecks();

        hostBuilder.AddSerilog(configuration, environment);

        return services;
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
    }


    #region Database

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, m => m.MigrationsHistoryTable("Migrations", "Core"))
                .ConfigureWarnings(builder =>
                {
                    builder.Ignore(CoreEventId
                        .PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                })
        );

        return services;
    }


    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        if (!databaseCreator.Exists())
        {
            Log.Information("Database does not exist. Creating it and applying migrations...");
            dbContext.Database.Migrate();
        }
        else
        {
            var connection = dbContext.Database.GetDbConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT TOP 1 * FROM Enum.Roles";

            try
            {
                command.ExecuteReader();
                Log.Information("Database already exists and is up to date.");
            }
            catch
            {
                Log.Information("Database exists but is not up to date. Applying migrations...");
                dbContext.Database.Migrate();
            }
        }
    }

    #endregion


    #region Authentication & Authorization

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtGenerator, JwtGenerator>();

        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
            throw new InvalidOperationException("JWT settings are not configured properly.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtSettings.Secret)
                    )
                };
            });

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        var builder = services.AddAuthorizationBuilder();

        foreach (var role in Enum.GetValues<RoleType>())
        {
            var roleName = Enum.GetName(role)!;

            builder.AddPolicy(roleName, policy => policy.RequireRole(roleName));
        }

        var permissions = typeof(Permissions)
            .GetFields()
            .Select(f => f.GetValue(null)?.ToString())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        foreach (var permission in permissions)
        {
            builder.AddPolicy(permission!, policy => policy.RequireClaim("Permission", permission!));
        }

        return services;
    }

    #endregion


    #region Email Settings

    private static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountEmails, AccountEmails>();

        var settings = new EmailSettings();
        configuration.Bind(nameof(EmailSettings), settings);

        services
            .AddFluentEmail(settings.Email, ProjectConstants.ProjectName)
            .AddSmtpSender(settings.Server, settings.Port, settings.Email, settings.Password);

        return services;
    }

    #endregion


    #region Serilog

    private static void AddSerilog(this IHostBuilder hostBuilder, IConfiguration configuration, IHostEnvironment env)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        hostBuilder.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration)
                .WriteTo.Console(env.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Error)
                .WriteTo.MSSqlServer(
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        SchemaName = "Core",
                        AutoCreateSqlTable = false
                    }
                )
        );
    }

    #endregion
}