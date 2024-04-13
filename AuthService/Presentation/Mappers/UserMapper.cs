using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Presentation.Mappers;

public static class UserMapper
{
    public static UserRequest MapToDto(IdentityUser identityUser)
    {
        return new UserRequest
        {
            Id = identityUser.Id,
            Email = identityUser.Email,
        };
    }
}