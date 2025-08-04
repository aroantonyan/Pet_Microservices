namespace ProductService.Contracts.Messaging.Interfaces;

public interface IDiscountPublisher
{
    Task PublishAsync(DiscountMessage discountMessage, CancellationToken cancellationToken = default);
}