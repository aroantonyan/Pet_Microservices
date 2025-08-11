using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductService.API.Options;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;

namespace ProductService.API.Extensions.Services;

public static class IdentityAuthExtensions
{
    public static IServiceCollection AddIdentityAndAuth(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((o, jwt) =>
            {
                var keyBytes = Encoding.UTF8.GetBytes(jwt.Value.Key!);
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = jwt.Value.Issuer,
                    ValidAudience            = jwt.Value.Audience,
                    IssuerSigningKey         = new SymmetricSecurityKey(keyBytes)
                };
            })
            .Validate(_ => true, "JWT validation is not configured")
            .ValidateOnStart();

        services.AddAuthorization();

        return services;
    }
}