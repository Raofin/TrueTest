using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OPS.Persistence;
using OPS.Persistence.Seeding;
using Serilog;

namespace OPS.Infrastructure.Database;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/> to apply database migrations and seed data.
/// </summary>
public static class Migration
{
    /// <summary>
    /// Applies database migrations and seeds initial data if the database does not exist or is outdated.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    public static void ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var databaseCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        try
        {
            if (!databaseCreator.Exists())
            {
                CreateDatabaseAndApplyMigrations(dbContext);
            }
            else
            {
                ApplyMigrations(dbContext);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error during database operation: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates the database, applies migrations, and seeds initial user data.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext"/> instance.</param>
    private static void CreateDatabaseAndApplyMigrations(AppDbContext dbContext)
    {
        dbContext.Database.Migrate();
        UserData.SeedUserData(dbContext);
    }

    /// <summary>
    /// Applies pending migrations and seeds initial user data if the database exists but is not up to date.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext"/> instance.</param>
    private static void ApplyMigrations(AppDbContext dbContext)
    {
        var connection = dbContext.Database.GetDbConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT TOP 1 * FROM Enum.Roles";

        try
        {
            command.ExecuteReader();
            Log.Information("Database is up to date.");
        }
        catch (Exception)
        {
            Log.Information("Database exists but is not up to date");
            dbContext.Database.Migrate();
            UserData.SeedUserData(dbContext);
        }
    }
}