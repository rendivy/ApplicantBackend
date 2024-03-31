using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Presentation.Mappers;

public static class UserMapper
{
    public static UserDTO MapToDto(IdentityUser identityUser)
    {
        return new UserDTO
        {
            Email = identityUser.Email,
            IsEmailConfirmed = identityUser.EmailConfirmed
        };
    }
}