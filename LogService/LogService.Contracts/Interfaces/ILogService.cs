using LogService.Contracts.Common;

namespace LogService.Contracts.Interfaces;

public interface ILogService
{
    public Task LogAsync(LogDto logDto);
}