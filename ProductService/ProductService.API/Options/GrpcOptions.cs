using System.ComponentModel.DataAnnotations;

namespace ProductService.API.Options;

public class GrpcOptions
{
    [Required, Url]
    public string? PriceServiceHost { get; set; } = null;
}