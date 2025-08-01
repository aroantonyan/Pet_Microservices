using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data;

namespace ProductService.Infrastructure.Authentication;

public class RefreshTokenService(IConfiguration config, AppDbContext dbContext)
{
    public async Task<RefreshToken> CreateRefreshTokenAsync(string userId)
    {
        var token = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        dbContext.RefreshTokens.Add(token);
        await dbContext.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string userId)
    {
        return await dbContext.RefreshTokens
            .OrderByDescending(r => r.ExpiryDate)
            .FirstOrDefaultAsync(r => r.UserId == userId && !r.IsRevoked);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwt || jwt.Header.Alg != SecurityAlgorithms.HmacSha256)
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public async Task SaveRefreshTokenAsync(RefreshToken token)
    {
        dbContext.RefreshTokens.Add(token);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task RevokeRefreshTokenAsync(RefreshToken token)
    {
        token.IsRevoked = true;
        dbContext.RefreshTokens.Update(token);
        await dbContext.SaveChangesAsync();
    }
}