using Microsoft.AspNetCore.Identity;

namespace ProductService.Domain.Entities;

public class User : IdentityUser
{
    public ICollection<RefreshToken> RefreshTokens { get; set; } 
}