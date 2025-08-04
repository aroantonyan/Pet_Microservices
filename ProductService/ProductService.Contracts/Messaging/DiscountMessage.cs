namespace ProductService.Contracts.Messaging;

public record DiscountMessage(Guid PriceId, decimal Discount);