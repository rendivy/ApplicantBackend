using System.IdentityModel.Tokens.Jwt;
using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Models;

namespace AuthService.Application.Services;

public class AccountService(AuthDbContext authDbContext) : IAccountService
{
    public Task<UserDTO> GetUserById(string userId)
    {
        var user = authDbContext.Users.FirstOrDefault(user => user.Id.ToString() == userId);
        var userDto = new UserDTO.UserDTOBuilder()
            .WithEmail(user.Email)
            .WithIsEmailConfirmed(user.EmailConfirmed)
            .Build();

        return Task.FromResult(userDto);
    }

    public async Task<TokenResponse> Login(LoginDTO loginDTO)
    {
        var user = authDbContext.Users.FirstOrDefault(user => user.Email == loginDTO.Email);
        var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken());
        var refreshToken = Guid.NewGuid().ToString();
        var expiresAt = DateTime.Now.AddMinutes(15);
        var tokenDto = new TokenResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
        };

        return await Task.FromResult(tokenDto);
    }

}