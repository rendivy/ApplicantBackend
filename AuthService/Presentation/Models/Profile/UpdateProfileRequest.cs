using AuthService.Domain.Entity;
using Common.BaseModel;

namespace AuthService.Presentation.Models.Profile;

public record UpdateProfileRequest
{
    public string Email { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public string FullName { get; init; }
    public string PhoneNumber { get; init; }
    public Gender Gender { get; init; }
    public string Citizenship { get; init; }
}