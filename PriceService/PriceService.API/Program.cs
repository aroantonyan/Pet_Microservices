using PriceService.API.Services.gRPC;
using PriceService.Infrastructure.Messaging;
using PriceService.Infrastructure.Persistence;
using Shared.Messaging.Abstractions;
using Shared.Messaging.Options;
using Shared.Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("Rabbit"));
builder.Services.AddSingleton<RabbitMqConnectionService>();
builder.Services.AddHostedService(sp=>sp.GetRequiredService<RabbitMqConnectionService>());
builder.Services.AddSingleton<IRabbitMqConnection>(sp=>sp.GetRequiredService<RabbitMqConnectionService>());
builder.Services.AddHostedService<DiscountConsumer>();
builder.Services.AddSingleton<RabbitMqConnectionService>();


var app = builder.Build();

app.MapGrpcService<PriceGrpcService>();

await StartupDbInitializer.CheckPriceTable(
    builder.Configuration.GetConnectionString("PriceDb")!,
    app.Lifetime.ApplicationStopping);

app.Run();