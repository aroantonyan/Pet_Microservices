using LogService.Contracts.Common;
using LogService.Contracts.Interfaces;
using Serilog;

namespace LogService.API.Services;

public class LoggingService : ILogService
{
    public Task LogAsync(LogDto log)
    {
        Log.Logger.Information(
            "Log received: Timestamp='{Timestamp:O}', Message='{Message}', Level='{Level}', " +
            "Source='{Source}', Path='{Path}', TraceId='{TraceId}', Exception='{Exception}'",
            log.Timestamp,         
            log.Message,
            log.Level,
            log.Source,
            log.Path,
            log.TraceId,
            log.Exception
        );

        return Task.CompletedTask;
    }
}