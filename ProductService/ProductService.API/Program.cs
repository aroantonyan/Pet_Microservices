using ProductService.API.Extensions;
using ProductService.API.Extensions.Services;
using ProductService.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAppOptions(builder.Configuration)
    .AddCaching()
    .AddGrpcClients()
    .AddHttpClients()
    .AddPersistence(builder.Configuration)
    .AddIdentityAndAuth()
    .AddApplicationServices()
    .AddInfrastructure(builder.Configuration); 

var app = builder.Build();

app.UseAppPipeline();

app.Run();