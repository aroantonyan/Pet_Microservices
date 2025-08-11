using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Shared.Messaging.Abstractions;

namespace PriceService.Infrastructure.Messaging;

public class DiscountConsumer(IRabbitMqConnection connection) : BackgroundService
{
    private const string QueueName = "discount_queue";
    private IChannel? Channel;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}