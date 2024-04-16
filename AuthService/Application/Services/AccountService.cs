using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Mappers;
using AuthService.Presentation.Models;
using AuthService.Presentation.Models.Account;
using AuthService.Presentation.Models.Token;
using Common.Exception;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class AccountService(AuthDbContext authDbContext, UserManager<User> userManager, JwtProvider jwtProvider)
    : IAccountService
{
    public async Task<UserRequest> GetUserById(string? userId)
    {
        if (userId == null) throw new UserNotFoundException("User not found");
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new UserNotFoundException("User not found");

        var roles = await userManager.GetRolesAsync(user);
        return UserMapper.MapToDto(user, roles.FirstOrDefault());
    }

    public async Task AddRole(string currentUserId, string userId, Roles role)
    {
        var current = await userManager.FindByIdAsync(currentUserId);
        var currentRoles = await userManager.GetRolesAsync(current);
        if (current != null && currentRoles.FirstOrDefault() != Roles.Admin.ToString())
            throw new UserDoesntHavePermissionException("You don't have permission to add role");
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new UserNotFoundException("User not found");
        var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        if (userRole == role.ToString()) throw new UserAlreadyHaveRoleException("User already has this role");

        await userManager.AddToRoleAsync(user, role.ToString());
    }


    public async Task<TokenResponse> Registration(RegistrationRequest registrationRequest)
    {
        var user = new User
        {
            UserName = registrationRequest.Email,
            Email = registrationRequest.Email,
            FullName = registrationRequest.FullName,
            PhoneNumber = registrationRequest.PhoneNumber,
            Citizenship = registrationRequest.Citizenship,
            DateOfBirth = registrationRequest.DateOfBirth,
            Gender = registrationRequest.Gender
        };

        var result = await userManager.CreateAsync(user, registrationRequest.Password);
        if (!result.Succeeded)
        {
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        await userManager.AddToRoleAsync(user, Roles.Applicant.ToString());
        var response = jwtProvider.CreateTokenResponse(new Guid(user.Id), Roles.Applicant.ToString());
        await authDbContext.UserRefreshTokens.AddAsync(new UserRefreshTokens
        {
            UserId = user.Id,
            RefreshToken = response.RefreshToken
        });
        await authDbContext.SaveChangesAsync();
        return response;
    }

    public async Task<TokenResponse> Login(LoginRequest loginRequest)
    {
        var user = authDbContext.Users.FirstOrDefault(user => user.Email == loginRequest.Email);
        if (user == null) throw new UserNotFoundException("User not found");
        var passwordCheck = await userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (!passwordCheck) throw new InvalidPasswordException("Invalid password");
        var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var response = jwtProvider.CreateTokenResponse(new Guid(user.Id), userRole);
        await authDbContext.UserRefreshTokens.AddAsync(new UserRefreshTokens
        {
            UserId = user.Id,
            RefreshToken = response.RefreshToken
        });
        await authDbContext.SaveChangesAsync();
        return response;
    }
}