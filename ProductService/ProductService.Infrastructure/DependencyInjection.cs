using Microsoft.Extensions.DependencyInjection;
using ProductService.Contracts.Messaging.Interfaces;
using ProductService.Infrastructure.Authentication;
using ProductService.Infrastructure.Messaging;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<RabbitMqConnectionService>();
        services.AddHostedService(sp => sp.GetRequiredService<RabbitMqConnectionService>());
        services.AddSingleton<IRabbitMqConnection>(sp =>
            sp.GetRequiredService<RabbitMqConnectionService>());
        services.AddSingleton<IDiscountPublisher, DiscountPublisher>();

        services.AddScoped<AccessTokenGenerator>();
        services.AddScoped<RefreshTokenService>();

        return services;
    }
}