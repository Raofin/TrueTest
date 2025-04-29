using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OPS.Persistence;
using OPS.Persistence.Seeding;
using Serilog;

namespace OPS.Infrastructure.Database;

public static class Migration
{
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

    private static void CreateDatabaseAndApplyMigrations(AppDbContext dbContext)
    {
        dbContext.Database.Migrate();
        UserData.SeedUserData(dbContext);
    }

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