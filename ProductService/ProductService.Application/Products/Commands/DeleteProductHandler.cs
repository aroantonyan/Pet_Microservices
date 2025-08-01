using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceContracts;
using ProductService.Contracts.Common;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;

namespace ProductService.Application.Products.Commands;

public class DeleteProductCommandHandler(AppDbContext context, PriceService.PriceServiceClient priceService) : IRequestHandler<DeleteProductCommand, RequestResponseDto<Product>>
{
    public async Task<RequestResponseDto<Product>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductName == command.ProductName,cancellationToken);
        if (product is null)
            return new RequestResponseDto<Product>() { IsSuccess = false, ErrorMessage = "Product not found" };
        var result = await priceService.DeletePriceAsync(
            new DeletePriceRequest { PriceId = product.PriceId.ToString() },
            new CallOptions(cancellationToken: cancellationToken)); 
        // if (!result.Success) ProductEvents.LogMessage($"PriceId{product.PriceId} was not deleted from price db");
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
        return new RequestResponseDto<Product>() { IsSuccess = true };
    }
}
