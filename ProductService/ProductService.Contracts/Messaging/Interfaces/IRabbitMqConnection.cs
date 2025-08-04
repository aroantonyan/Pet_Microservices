using RabbitMQ.Client;

namespace ProductService.Contracts.Messaging.Interfaces;

public interface IRabbitMqConnection : IAsyncDisposable
{
    Task<IChannel>  CreateChannelAsync(CancellationToken cancellationToken = default);
}