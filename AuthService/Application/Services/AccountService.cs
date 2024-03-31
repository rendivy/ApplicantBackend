using AuthService.Application.Interfaces;
using AuthService.Domain.Entity;
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
}