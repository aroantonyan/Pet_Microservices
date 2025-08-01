using MediatR;
using ProductService.Contracts.Authentication;
using ProductService.Contracts.Common;

namespace ProductService.Application.Authentication;

public record RefreshTokenCommand(RefreshTokenRequestDto RefreshTokenRequestDto):IRequest<RequestResponseDto<RefreshTokenResponseDto>>;