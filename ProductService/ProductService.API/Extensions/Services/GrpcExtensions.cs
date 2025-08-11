using Microsoft.Extensions.Options;
using PriceContracts;
using ProductService.API.Options;

namespace ProductService.API.Extensions.Services;

public static class GrpcExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services)
    {
        services.AddGrpcClient<PriceService.PriceServiceClient>((sp, o) =>
        {
            var options = sp.GetRequiredService<IOptions<GrpcOptions>>().Value;
            o.Address = new Uri(options.PriceServiceHost!);
        });
        return services;
    }
}