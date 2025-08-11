using System.ComponentModel.DataAnnotations;

namespace ProductService.API.Options;

public class LogServiceOptions
{
    [Required, Url]
    public string? LogServiceHost { get; set; } = null;
}