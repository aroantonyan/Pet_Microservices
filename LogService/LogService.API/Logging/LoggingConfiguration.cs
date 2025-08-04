using Serilog;
using Serilog.Events;

namespace LogService.API.Logging;

public class LoggingConfiguration
{
    public static void ConfigureLogger(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(
                path: "Logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10,
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}"
            )
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SourceContext") &&
                                             e.Properties["SourceContext"].ToString()
                                                 .Contains("Microsoft.Hosting.Lifetime"))
                .WriteTo.Console(outputTemplate: "{Message:lj}{NewLine}")
            )
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger, dispose: true);
    }
}