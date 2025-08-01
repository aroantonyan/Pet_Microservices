using MediatR;
using ProductService.Contracts.Authentication;
using ProductService.Contracts.Common;

namespace ProductService.Application.Authentication;

public record LoginCommand(LoginDto LoginDto ) :  IRequest<RequestResponseDto<RefreshAccessTokenDto>>;