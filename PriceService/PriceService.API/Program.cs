using PriceService.API.Services.gRPC;
using PriceService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<PriceGrpcService>();

await StartupDbInitializer.CheckPriceTable(
    builder.Configuration.GetConnectionString("PriceDb")!,
    app.Lifetime.ApplicationStopping);

app.Run();