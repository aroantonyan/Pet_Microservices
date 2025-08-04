using System.Text;
using System.Text.Json;
using ProductService.Contracts.Logging;

namespace ProductService.Infrastructure.Logging;

public class LogSenderService(HttpClient client)
{
    private const string Endpoint = "/log/ReceiveLog";   

    public async Task SendLogAsync(LogDto log)
    {
        var payload  = JsonSerializer.Serialize(log);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var resp = await client.PostAsync(Endpoint, content);
        resp.EnsureSuccessStatusCode();
    }
}