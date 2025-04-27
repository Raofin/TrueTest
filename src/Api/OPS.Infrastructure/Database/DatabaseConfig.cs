using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OPS.Persistence;

namespace OPS.Infrastructure.Database;

public static class DatabaseServices
{
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