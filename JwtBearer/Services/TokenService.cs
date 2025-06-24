using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using JwtBearer.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JwtBearer.Services;

public class TokenService
{
     private readonly IConfiguration _configuration;

     public TokenService(IConfiguration configuration)
     {
        _configuration = configuration;
     }
    public string Generate(User user) 
    {
        // Cria uma instância do JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();

        var privateKey = _configuration["Jwt:PrivateKey"];
        
        if (string.IsNullOrEmpty(privateKey))
        {
            throw new InvalidOperationException("A chave 'Jwt:PrivateKey' não foi encontrada no appsettings.json.");
        }

        var key = Encoding.UTF8.GetBytes(privateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),

        };

        // Gera um token
        var token = handler.CreateToken(tokenDescriptor);

        // Gera um string do token
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(
            new Claim(ClaimTypes.Name, user.Email));
        foreach (var role in user.Roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
        
        ci.AddClaim(new Claim("Fruta", "Banana"));

        return ci;
    }
}