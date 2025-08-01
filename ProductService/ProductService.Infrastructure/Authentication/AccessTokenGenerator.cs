using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ProductService.Infrastructure.Authentication;

public sealed class AccessTokenGenerator(IConfiguration config)
{
    public string GenerateAccessToken(IdentityUser user)
    {
        var jwt = config.GetSection("Jwt");

        var key       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds     = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresIn = DateTime.UtcNow.AddMinutes(
            double.Parse(jwt["ExpiresInMinutes"] ?? "60"));

        var token = new JwtSecurityToken(
            issuer:  jwt["Issuer"],
            audience: jwt["Audience"],  
            claims:  [ new Claim(ClaimTypes.NameIdentifier, user.Id) ],
            expires: expiresIn,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}