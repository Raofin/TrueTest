using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace OPS.Infrastructure.AppConfiguration.Logging;

internal static class SerilogServices
{
    public static void AddSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var logLevels = new LogSettings();
        configuration.GetSection("Logging:LogLevels").Bind(logLevels);

        var consoleLevel = ParseLogLevel(logLevels.Console, LogEventLevel.Information);
        var databaseLevel = ParseLogLevel(logLevels.Database, LogEventLevel.Warning);

        hostBuilder.UseSerilog((context, config) =>
            config
                .WriteTo.Console(consoleLevel)
                .WriteTo.MSSqlServer(
                    restrictedToMinimumLevel: databaseLevel,
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

    private static LogEventLevel ParseLogLevel(string level, LogEventLevel fallback)
    {
        return Enum.TryParse<LogEventLevel>(level, true, out var parsed)
            ? parsed
            : fallback;
    }
}