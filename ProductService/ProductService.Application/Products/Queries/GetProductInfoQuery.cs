using MediatR;
using ProductService.Contracts.Common;
using ProductService.Contracts.Products.Dtos;

namespace ProductService.Application.Products.Queries;

public record GetProductInfoQuery(string ProductName) : IRequest<RequestResponseDto<ProductResponseDto>>;