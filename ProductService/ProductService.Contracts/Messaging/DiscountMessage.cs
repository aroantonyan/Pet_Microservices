namespace ProductService.Contracts.Messaging;

public record DiscountMessage(Guid PriceId, string Discount);