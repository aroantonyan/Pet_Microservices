namespace ProductService.API.Extensions.Services;

public static class CacheExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(_ => {});
        return services;
    }
}