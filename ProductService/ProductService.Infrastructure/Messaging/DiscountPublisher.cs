using System.Text.Json;
using ProductService.Contracts.Messaging;
using ProductService.Contracts.Messaging.Interfaces;
using RabbitMQ.Client;

namespace ProductService.Infrastructure.Messaging;

public sealed class DiscountPublisher : IDiscountPublisher, IAsyncDisposable
{
    private const string QueueName = "discount_queue";

    private readonly Lazy<Task<IChannel>> _lazyChannel;

    public DiscountPublisher(IRabbitMqConnection connection)
    {
        _lazyChannel = new Lazy<Task<IChannel>>(async () =>
        {
            var ch = await connection.CreateChannelAsync();

            await ch.QueueDeclareAsync(
                queue      : QueueName,
                durable    : true,
                exclusive  : false,
                autoDelete : false,
                arguments  : null);

            return ch;
        });
    }

    public async Task PublishAsync(DiscountMessage message, CancellationToken ct = default)
    {
        var channel = await _lazyChannel.Value;

        var props = new BasicProperties()
        {
            Persistent = true,
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
        };

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        await channel.BasicPublishAsync(
            exchange        : "",      
            routingKey      : QueueName,
            mandatory       : false,
            basicProperties: props,
            body,
            ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_lazyChannel.IsValueCreated)
        {
            var ch = await _lazyChannel.Value;
            await ch.CloseAsync();
            ch.Dispose();
        }
    }
}