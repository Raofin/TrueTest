using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OPS.Persistence;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace OPS.Infrastructure;

public static class AppConfigurations
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services
            .AddDatabase(configuration)
            .AddHttpContextAccessor()
            .AddMemoryCache()
            .AddHealthChecks();
    }

    public static void UseInfrastructure(this IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseSerilogRequestLogging();

        if (env.IsDevelopment())
        {
            app.ApplyMigration();
        }
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
            connectionString,
            m => m.MigrationsHistoryTable("Migrations"))
        );

        return services;
    }

    private static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        if (!databaseCreator.Exists())
        {
            dbContext.Database.Migrate();
        }
    }

    public static void AddSerilog(this IHostBuilder hostBuilder, IConfiguration configuration, IHostEnvironment env)
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
                        AutoCreateSqlTable = false
                    }
                )
        );
    }
}
