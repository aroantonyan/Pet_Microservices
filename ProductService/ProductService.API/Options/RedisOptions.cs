using System.ComponentModel.DataAnnotations;

namespace ProductService.API.Options;

public class RedisOptions
{
    [Required]
    public string? ConnectionString { get; init; } = null;
}