using RabbitMQ.Client;

namespace Shared.Messaging.Abstractions;

public interface IRabbitMqConnection : IAsyncDisposable
{
    Task<IChannel> CreateChannelAsync();

}