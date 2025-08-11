using FluentValidation;
using FluentValidation.AspNetCore;
using ProductService.Application.Authentication;

namespace ProductService.API.Extensions.Services;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoginCommand>();

        services.AddControllers();

        services.AddSwaggerWithJwt(); 

        return services;
    }
}