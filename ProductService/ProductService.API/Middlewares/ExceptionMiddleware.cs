using System.Net;
using System.Text.Json;
using ProductService.Contracts.Common;
using ProductService.Contracts.Logging;

namespace ProductService.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = new RequestResponseDto<object>
            {
                IsSuccess = false,
                ErrorMessage = ex.Message,
                Data = new
                {
                    ExceptionType = ex.GetType().Name,
                    context.Request.Path,
                    TraceId = context.TraceIdentifier,
                    ex.StackTrace
                }
            };

            var log = new LogDto
            {
                Message = "Unhandled exception",
                Level = "Error",
                Source = "ProductService",
                Timestamp = DateTime.UtcNow,
                Exception = ex.ToString(),
                Path = context.Request.Path,
                TraceId = context.TraceIdentifier
            };

            //await logger.SendLogAsync(log);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}