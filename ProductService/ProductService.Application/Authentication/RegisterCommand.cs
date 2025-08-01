using MediatR;
using ProductService.Contracts.Authentication;
using ProductService.Contracts.Common;

namespace ProductService.API.Authentication;

public record RegisterCommand(RegisterDto RegisterDto): IRequest<RequestResponseDto<string>>;