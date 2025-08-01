namespace ProductService.Contracts.Logging;

public class LogDto
{
    public string? Message { get; set; } 
    public string? Level { get; set; } 
    public string? Source { get; set; } 
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Exception { get; set; }
    public string? Path { get; set; }
    public string? TraceId { get; set; }
}