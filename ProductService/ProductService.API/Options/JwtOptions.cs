using System.ComponentModel.DataAnnotations;

namespace ProductService.API.Options;

public sealed class JwtOptions
{
    [Required]
    public string? Issuer { get; set; } = null;
    [Required]
    public string? Audience { get; set; } = null;
    [Required]
    [MinLength(16)]
    public string? Key { get; set; } = null;
}