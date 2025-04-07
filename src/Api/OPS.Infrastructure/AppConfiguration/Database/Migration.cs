using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using OPS.Persistence;
using Serilog;

namespace OPS.Infrastructure.AppConfiguration.Database;

public static class Migration
{
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
}