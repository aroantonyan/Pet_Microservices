using Microsoft.Extensions.Options;
using ProductService.API.Options;
using ProductService.Infrastructure.Logging;

namespace ProductService.API.Extensions.Services;

public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<LogSenderService>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<LogServiceOptions>>().Value;
            client.BaseAddress = new Uri(options.LogServiceHost!);
        });
        return services;
    }
}