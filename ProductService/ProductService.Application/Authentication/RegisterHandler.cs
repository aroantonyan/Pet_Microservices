using MediatR;
using Microsoft.AspNetCore.Identity;
using ProductService.Contracts.Common;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Authentication;

namespace ProductService.API.Authentication;

public sealed class RegisterHandler(UserManager<User> userManager, RefreshTokenService refreshTokenService)
    : IRequestHandler<RegisterCommand, RequestResponseDto<string>>
{
    public async Task<RequestResponseDto<string>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var alreadyRegistered = await userManager.FindByEmailAsync(command.RegisterDto.Email!);
        if (alreadyRegistered != null)
            return new RequestResponseDto<string>()
                { IsSuccess = false, ErrorMessage = "User with provided email already exists." };
        var user = new User()
        {
            UserName = command.RegisterDto.Username,
            Email = command.RegisterDto.Email,
        };
        var result = await userManager.CreateAsync(user, command.RegisterDto.Password!);
        if (result.Succeeded)
            return new RequestResponseDto<string>() { IsSuccess = false, ErrorMessage = "Problems with creating user" };

        await refreshTokenService.CreateRefreshTokenAsync(user.Id);
        return new RequestResponseDto<string>
        {
            IsSuccess = true,
            Data = "User created"
        };
    }
}