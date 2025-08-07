using Hangfire;
using LogService.Contracts.Common;
using LogService.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogService.API.Controllers;

[ApiController]
[Route("log")]
public class LogController : ControllerBase
{
    [HttpPost("ReceiveLog")]
    public IActionResult ReceiveLog([FromBody] LogDto log)
    {
        BackgroundJob.Enqueue<ILogService>(logger => logger.LogAsync(log) );
        return Ok();
    }
}