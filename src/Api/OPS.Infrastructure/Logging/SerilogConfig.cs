using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace OPS.Infrastructure.Logging;

/// <summary>
/// Extension methods for <see cref="IHostBuilder"/> to configure Serilog for logging.
/// </summary>
internal static class SerilogConfig
{
    /// <summary>
    /// Configures Serilog as the logging provider for the host.
    /// </summary>
    /// <param name="hostBuilder">The <see cref="IHostBuilder"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings, including logging configuration.</param>
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

    /// <summary>
    /// Parses a log level string to its <see cref="LogEventLevel"/> enum value.
    /// Returns a fallback value if parsing fails.
    /// </summary>
    /// <param name="level">The log level string to parse.</param>
    /// <param name="fallback">The <see cref="LogEventLevel"/> to return if parsing fails.</param>
    /// <returns>The parsed <see cref="LogEventLevel"/> or the fallback value.</returns>
    private static LogEventLevel ParseLogLevel(string level, LogEventLevel fallback)
    {
        return Enum.TryParse<LogEventLevel>(level, true, out var parsed)
            ? parsed
            : fallback;
    }
}