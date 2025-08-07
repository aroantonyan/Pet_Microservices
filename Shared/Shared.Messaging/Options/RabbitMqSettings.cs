namespace Shared.Messaging.Options;

public record RabbitMqSettings
{
    public string Host { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;

    public RabbitMqSettings() { }
}