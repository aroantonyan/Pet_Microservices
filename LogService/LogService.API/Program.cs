using Hangfire;
using Hangfire.PostgreSql;
using LogService.API.Logging;
using LogService.API.Services;
using LogService.Contracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);

LoggingConfiguration.ConfigureLogger(builder);

var cs = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(cfg => cfg.UsePostgreSqlStorage(cs));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ILogService, LoggingService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard();

app.Run();