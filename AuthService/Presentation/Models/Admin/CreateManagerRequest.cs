using System.ComponentModel.DataAnnotations;
using Common.BaseModel;

namespace AuthService.Presentation.Models.Admin;

public class CreateManagerRequest
{
    [EmailAddress] 
    public required string Email { get; set; }
    [MinLength(2)] 
    public required string FullName { get; set; }
    [Phone] 
    public required string PhoneNumber { get; set; }
    [MinLength(2)] 
    public required string Citizenship { get; set; }
    public required Gender Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }
}