using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Presentation.Models.Token;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Application.Services;

public class JwtProvider(IConfiguration configuration)
{
    public TokenResponse CreateTokenResponse(Guid id, string role)
    {
        return new TokenResponse
        {
            AccessToken = CreateAccessToken(id, role),
            RefreshToken = CreateRefreshToken()
        };
    }


    private string CreateAccessToken(Guid id, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("AppSettings:SecretKey")!);
        var expireMinutes = configuration.GetValue<double>("AppSettings:AccessTokenExpireMinutes");
        var issuer = configuration.GetValue<string>("AppSettings:Issuer")!;
        var tokenId = Guid.NewGuid();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, id.ToString()),
                new(ClaimTypes.NameIdentifier, tokenId.ToString()),
                new(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = issuer,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string CreateRefreshToken()
    {
        return GenerateRandomString();
    }

    private static string GenerateRandomString()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128));
    }
}