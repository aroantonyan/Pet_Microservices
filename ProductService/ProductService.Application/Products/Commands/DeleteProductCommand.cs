using MediatR;
using ProductService.Contracts.Common;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands;

public record DeleteProductCommand(string ProductName) : IRequest<RequestResponseDto<Product>>;