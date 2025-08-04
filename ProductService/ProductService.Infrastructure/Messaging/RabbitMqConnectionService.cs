using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using ProductService.Contracts.Messaging.Interfaces;

namespace ProductService.Infrastructure.Messaging;

public sealed class RabbitMqConnectionService(IConfiguration configuration) : IHostedService, IRabbitMqConnection
{   
    private IConnection? _connection;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName                 = configuration["Rabbit:Host"] ?? "localhost",
            AutomaticRecoveryEnabled = true
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
    }

    public Task<IChannel> CreateChannelAsync()
    {
        if (_connection is null)
            throw new InvalidOperationException("RabbitMQ connection has not been established.");

        return _connection.CreateChannelAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _connection?.CloseAsync(cancellationToken);
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.CloseAsync();
    }
}