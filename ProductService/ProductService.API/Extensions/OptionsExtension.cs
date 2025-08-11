using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using ProductService.API.Options;

namespace ProductService.API.Extensions;

public static class OptionsExtension
{
    public static IServiceCollection AddAppOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt"))
            .ValidateDataAnnotations()
            .Validate(o => o.Key!.Length >= 16, "Jwt:Key must be >= 16 chars")
            .ValidateOnStart();

        services.AddOptions<GrpcOptions>()
            .Bind(configuration.GetSection("Grpc"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<LogServiceOptions>()
            .Bind(configuration.GetSection("LogService"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RedisOptions>()
            .Bind(configuration.GetSection("Redis"))
            .ValidateDataAnnotations()
            .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString), "Redis:ConnectionString is required")
            .ValidateOnStart();


        services.AddOptions<RedisCacheOptions>()
            .Configure<IOptions<RedisOptions>>((o, src) =>
            {
                var r = src.Value;
                o.Configuration = r.ConnectionString;
            });


        return services;
    }
}