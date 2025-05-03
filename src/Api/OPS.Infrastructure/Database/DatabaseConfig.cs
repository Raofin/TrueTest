using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Persistence;

namespace OPS.Infrastructure.Database;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to add database related services.
/// </summary>
public static class DatabaseServices
{
    /// <summary>
    /// Adds the <see cref="AppDbContext"/> to the service collection using SQL Server.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings, including the database connection string.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    internal static IServiceCollection AddDatabaseServices
        (this IServiceCollection services, IConfiguration configuration)
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
}