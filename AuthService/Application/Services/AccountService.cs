using AuthService.Application.Interfaces;
using AuthService.Domain.Entity;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Models;
using Common.CustomException;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class AccountService(
    AuthDbContext authDbContext,
    UserManager<User> userManager,
    JwtProvider jwtProvider)
    : IAccountService
{
    public Task<UserRequest> GetUserById(string userId)
    {
        var user = userManager.FindByIdAsync(userId).Result;
        if (user == null) throw new UserNotFoundException("User not found");

        var userDto = new UserRequest
        {
            Id = user.Id,
            Email = user.Email,
        };

        return Task.FromResult(userDto);
    }


    public async Task<TokenResponse> Registration(RegistrationRequest registrationRequest)
    {
        var user = new User
        {
            UserName = registrationRequest.Email,
            Email = registrationRequest.Email,
        };

        

        var result = await userManager.CreateAsync(user, registrationRequest.Password);
        await userManager.AddToRoleAsync(user, Roles.Applicant.ToString());
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        return jwtProvider.CreateTokenResponse(new Guid(user.Id));
    }

    public async Task<TokenResponse> Login(LoginRequest loginRequest)
    {
        var user = authDbContext.Users.FirstOrDefault(user => user.Email == loginRequest.Email);
        if (user == null) throw new UserNotFoundException("User not found");
        var passwordCheck = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!passwordCheck) throw new InvalidPasswordException("Invalid password");
        return jwtProvider.CreateTokenResponse(new Guid(user.Id));
    }
}