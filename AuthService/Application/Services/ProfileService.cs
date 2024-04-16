using AuthService.Application.Services.Models.Profile;
using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Mappers;
using AuthService.Presentation.Models.Account;
using Common.Exception;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class ProfileService(AuthDbContext authDbContext, UserManager<User> userManager, JwtProvider jwtProvider)
    : IProfileService
{
    public async Task UpdateProfile(string userId, UpdateProfileRequest updateProfileRequest)
    {
        var user = await userManager.FindByIdAsync(userId);
        
        if (updateProfileRequest.Email != user.Email)
        {
            var userEmailExists = await userManager.FindByEmailAsync(updateProfileRequest.Email);
            if (userEmailExists != null)
                throw new Exception("User with this email already exist");
            user.Email = updateProfileRequest.Email;
        }

        if (updateProfileRequest.PhoneNumber != user.PhoneNumber)
        {
            var userPhoneNumberExists = await userManager.FindByEmailAsync(updateProfileRequest.PhoneNumber);
            if (userPhoneNumberExists != null)
                throw new Exception("User with this phone number already exist");
            user.PhoneNumber = updateProfileRequest.PhoneNumber;
        }

        user.FullName = updateProfileRequest.FullName;
        user.DateOfBirth = updateProfileRequest.DateOfBirth;
        user.Gender = updateProfileRequest.Gender;
        user.Citizenship = updateProfileRequest.Citizenship;
        
        await userManager.UpdateAsync(user);

    }

    public async Task<UserRequest> GetUserProfile(string userId)
    {
        if (userId == null) throw new UserNotFoundException("User not found");
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new UserNotFoundException("User not found");

        var roles = await userManager.GetRolesAsync(user);
        return UserMapper.MapToDto(user, roles.FirstOrDefault());
    }
}