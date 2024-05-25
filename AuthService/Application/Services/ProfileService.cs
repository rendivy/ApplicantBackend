using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Presentation.Mappers;
using AuthService.Presentation.Models.Profile;
using Common.Exception;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IBus _bus;
    private readonly UserManager<User> _userManager;


    public ProfileService(
        UserManager<User> userManager,
        IBus bus)
    {
        _bus = bus;
        _userManager = userManager;
    }


    public async Task UpdateProfile(string userId, UpdateProfileRequest updateProfileRequest)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (updateProfileRequest.Email != user.Email)
        {
            var userEmailExists = await _userManager.FindByEmailAsync(updateProfileRequest.Email);
            if (userEmailExists != null)
                throw new Exception("User with this email already exist");
            user.Email = updateProfileRequest.Email;
        }

        if (updateProfileRequest.PhoneNumber != user.PhoneNumber)
        {
            var userPhoneNumberExists = await _userManager.FindByEmailAsync(updateProfileRequest.PhoneNumber);
            if (userPhoneNumberExists != null)
                throw new Exception("User with this phone number already exist");
            user.PhoneNumber = updateProfileRequest.PhoneNumber;
        }

        user.FullName = updateProfileRequest.FullName;
        user.DateOfBirth = updateProfileRequest.DateOfBirth;
        user.Gender = updateProfileRequest.Gender;
        user.Citizenship = updateProfileRequest.Citizenship;
        await _userManager.UpdateAsync(user);
    }

    public async Task<UserRequest> GetUserProfile(string userId)
    {
        if (userId == null) throw new UserNotFoundException("User not found");
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) throw new UserNotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        return UserMapper.MapToDto(user, roles.FirstOrDefault());
    }
}