using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.IdentityService.Infrastructure.Security;

public sealed class JwtTokenFactory(IConfiguration configuration)
{
    public (string Token, DateTime ExpiresAtUtc) CreateToken(Guid userId, string fullName, string email, string role)
    {
        var issuer = configuration["Jwt:Issuer"] ?? "BookStore.Identity";
        var audience = configuration["Jwt:Audience"] ?? "BookStore.Client";
        var secret = configuration["Jwt:Secret"] ?? "bookstore-super-secret-key-for-local-dev-only";
        var expiresAtUtc = DateTime.UtcNow.AddHours(8);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new("name", fullName),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }
}
