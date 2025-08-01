using MediatR;
using PriceContracts;
using ProductService.Contracts.Common;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;

namespace ProductService.Application.Products.Commands;

public class CreateProductHandler(PriceService.PriceServiceClient priceServiceClient,
    AppDbContext context) : IRequestHandler<CreateProductCommand, RequestResponseDto<Product>>
{
    public async Task<RequestResponseDto<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var priceId = Guid.NewGuid();
        var product = new Product()
        {
            ProductDescription = command.Dto.ProductDescription,
            ProductName = command.Dto.ProductName,
            PriceId = priceId
        };
        await priceServiceClient.CreatePriceAsync(new CreatePriceRequest
        {
            PriceId = priceId.ToString(),
            Value = command.Dto.ProductPrice,
            Currency = "USD"
        }, cancellationToken: cancellationToken);
        
        await context.Products.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        // LogDelegate.LogToConsole("A request to create a product was sent");

        return new RequestResponseDto<Product>() { IsSuccess = true };
        
    }
}