using MediatR;
using ProductService.Contracts.Authentication;
using ProductService.Contracts.Common;

namespace ProductService.Application.Authentication;

public record RegisterCommand(RegisterDto RegisterDto): IRequest<RequestResponseDto<string>>;