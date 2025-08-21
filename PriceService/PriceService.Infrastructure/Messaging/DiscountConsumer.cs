using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceService.Contracts.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messaging.Abstractions;

namespace PriceService.Infrastructure.Messaging;

public sealed class DiscountConsumer(
    IRabbitMqConnection conn,
    ILogger<DiscountConsumer> log
) : BackgroundService, IAsyncDisposable
{
    private const string QueueName = "discount_queue";
    private IChannel? _ch;
    private string? _consumerTag;

    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ch = await conn.CreateChannelAsync().ConfigureAwait(false);

        await _ch.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null, cancellationToken: stoppingToken).ConfigureAwait(false);

        await _ch.BasicQosAsync(prefetchSize: 0, prefetchCount: 32, global: false, cancellationToken: stoppingToken)
            .ConfigureAwait(false);

        var consumer = new AsyncEventingBasicConsumer(_ch); // принимает IChannel
        consumer.ReceivedAsync += OnReceivedAsync;
        consumer.ShutdownAsync += (_, ea) =>
        {
            log.LogWarning("DiscountConsumer shutdown: {Code} {Text}", ea.ReplyCode, ea.ReplyText);
            return Task.CompletedTask;
        };

        _consumerTag = await _ch.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer, cancellationToken: stoppingToken).ConfigureAwait(false);
    }

    private async Task OnReceivedAsync(object sender, BasicDeliverEventArgs ea)
    {
        // Важно: копируем тело до await — после возврата буфер может быть освобождён
        var body = ea.Body.ToArray();

        try
        {
            var msg = JsonSerializer.Deserialize<DiscountMessage>(body, Json)
                      ?? throw new JsonException("DiscountMessage deserialization returned null");

            Console.WriteLine("balbal");
            Console.WriteLine("balbal");
            Console.WriteLine("balbal");


            await _ch!.BasicAckAsync(ea.DeliveryTag, multiple: false).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to process DiscountMessage, requeueing");
            await _ch!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true)
                .ConfigureAwait(false);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_ch is null) return;

            if (_consumerTag is not null)
            {
                try
                {
                    await _ch.BasicCancelAsync(_consumerTag).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }

            try
            {
                await _ch.CloseAsync().ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }

            (_ch)?.Dispose();
        }
        finally
        {
            _consumerTag = null;
            _ch = null;
        }
    }
}