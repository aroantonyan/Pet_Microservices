using MediatR;
using ProductService.Contracts.Common;
using ProductService.Contracts.Products.Dtos;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<RequestResponseDto<Product>>;