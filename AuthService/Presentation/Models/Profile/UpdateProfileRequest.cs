using AuthService.Domain.Entity;

namespace AuthService.Application.Services.Models.Profile;

public record UpdateProfileRequest
{
    public string Email { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string FullName { get; init; }
    public string PhoneNumber { get; init; }
    public Gender Gender { get; init; }
    public string Citizenship { get; init; }
}