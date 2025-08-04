using Microsoft.Extensions.DependencyInjection;
using ProductService.Infrastructure.Authentication;

namespace ProductService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<AccessTokenGenerator>();
        services.AddScoped<RefreshTokenService>();

        return services;
    }
}