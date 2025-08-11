using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PriceContracts;
using ProductService.Contracts.Common;
using ProductService.Contracts.Messaging;
using ProductService.Contracts.Messaging.Interfaces;
using ProductService.Contracts.Products.Dtos;
using ProductService.Infrastructure.Data;

namespace ProductService.Application.Products.Queries;

using System.Globalization;
using MediatR;


public class GetProductInfoHandler(
    AppDbContext context,
    IDistributedCache cache,
    PriceService.PriceServiceClient priceServiceClient,
    IDiscountPublisher  discountPublisher)
    : IRequestHandler<GetProductInfoQuery, RequestResponseDto<ProductResponseDto>>
{
    public async Task<RequestResponseDto<ProductResponseDto>> Handle(GetProductInfoQuery query,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.ProductName == query.ProductName, cancellationToken);
        if (product is null)
            return new RequestResponseDto<ProductResponseDto>
            {
                IsSuccess = false,
                ErrorMessage = "Product not found"
            };
        var cacheKey = $"price:{product.PriceId}";
        var cached = await cache.GetStringAsync(cacheKey, cancellationToken);

        await discountPublisher.PublishAsync(new DiscountMessage(Guid.NewGuid(), "myau"), cancellationToken);

        if (cached is not null)
        {
            return new RequestResponseDto<ProductResponseDto>
            {
                IsSuccess = true,
                Data = new ProductResponseDto()
                {
                    ProductPrice = double.Parse(cached, CultureInfo.InvariantCulture),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                }
            };
        }

        // LogDelegate.LogToConsole("a request to get a product was sent");
        // ProductEvents.LogMessage("a request to get a product was sent");
        var priceResponse = await priceServiceClient.GetPriceByIdAsync(new GetPriceByIdRequest
            { PriceId = product.PriceId.ToString() }, cancellationToken: cancellationToken);
        if (priceResponse.Found)
        {
            await cache.SetStringAsync(cacheKey, priceResponse.Value.ToString(CultureInfo.InvariantCulture),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                },cancellationToken);
            return new RequestResponseDto<ProductResponseDto>
            {
                IsSuccess = true,
                Data = new ProductResponseDto()
                {
                    ProductPrice = double.Parse(priceResponse.Value.ToString(CultureInfo.InvariantCulture),
                        CultureInfo.InvariantCulture),
                    ProductDescription = product.ProductDescription,
                    ProductName = product.ProductName,
                }
            };
        }

        return new RequestResponseDto<ProductResponseDto>()
        {
            IsSuccess = true,
            ErrorMessage = "Price not found",
            Data = new ProductResponseDto()
            {
                ProductPrice = -1,
                ProductDescription = product.ProductDescription,
                ProductName = product.ProductName,
            }
        };
    }
}