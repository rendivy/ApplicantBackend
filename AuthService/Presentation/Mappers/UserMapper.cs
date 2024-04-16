using AuthService.Domain.Entity;
using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Presentation.Mappers;

public static class UserMapper
{
    public static UserRequest MapToDto(User identityUser, string? roles)
    {
        return new UserRequest
        {
            Id = identityUser.Id,
            Email = identityUser.Email,
            Role = roles,
            Gender = identityUser.Gender,
            FullName = identityUser.FullName,
            PhoneNumber = identityUser.PhoneNumber,
            Citizenship = identityUser.Citizenship,
            DateOfBirth = identityUser.DateOfBirth
        };
    }
}