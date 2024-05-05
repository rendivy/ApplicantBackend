using AuthService.Presentation.Models.Account;
using AuthService.Presentation.Models.Profile;

namespace AuthService.Domain.Interfaces;

public interface IProfileService
{
    public Task UpdateProfile(string userId, UpdateProfileRequest updateProfileRequest);

    public Task<UserRequest> GetUserProfile(string userId);
}