using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Messaging.Abstractions;
using Shared.Messaging.Options;

namespace Shared.Messaging.RabbitMQ;

public sealed class RabbitMqConnectionService(
    IOptions<RabbitMqSettings> opt) 
    : IHostedService, IRabbitMqConnection
{
    private readonly RabbitMqSettings _cfg = opt.Value;
    private IConnection? _conn;

    public async Task StartAsync(CancellationToken ct)
    {
        var factory = new ConnectionFactory
        {
            HostName  = _cfg.Host,
            UserName  = _cfg.User,
            Password  = _cfg.Password,
            AutomaticRecoveryEnabled = true
        };

        _conn = await factory.CreateConnectionAsync(ct);
    }

    public Task<IChannel> CreateChannelAsync()
        => _conn is null
            ? throw new InvalidOperationException("RabbitMQ not connected")
            : _conn.CreateChannelAsync();

    public Task StopAsync(CancellationToken ct)
    {
        _conn?.CloseAsync(ct);
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync() =>
        _conn?.DisposeAsync() ?? ValueTask.CompletedTask;
}