using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;

namespace ProductService.API.Extensions.Services;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("ProductDb"));
        });
        return services;
    }
}