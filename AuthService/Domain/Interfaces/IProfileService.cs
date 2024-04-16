using AuthService.Application.Services.Models.Profile;
using AuthService.Presentation.Models.Account;

namespace AuthService.Domain.Interfaces;

public interface IProfileService
{
    public Task UpdateProfile(string userId, UpdateProfileRequest updateProfileRequest);

    public Task<UserRequest> GetUserProfile(string userId);
}