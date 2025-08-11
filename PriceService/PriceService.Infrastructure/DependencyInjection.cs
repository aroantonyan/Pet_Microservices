using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Messaging.Abstractions;
using Shared.Messaging.Options;
using Shared.Messaging.RabbitMQ;

namespace PriceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("rabbit"));
        services.AddSingleton<RabbitMqConnectionService>();
        services.AddHostedService(sp => sp.GetRequiredService<RabbitMqConnectionService>());
        services.AddSingleton<IRabbitMqConnection>(sp =>
            sp.GetRequiredService<RabbitMqConnectionService>());
        return services;
    }
}