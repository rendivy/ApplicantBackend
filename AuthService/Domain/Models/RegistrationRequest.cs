using System.ComponentModel.DataAnnotations;
using AuthService.Domain.Entity;

namespace AuthService.Presentation.Models;

public class RegistrationRequest
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] [MinLength(2)] public string FullName { get; set; }
    [Phone] [Required] public string PhoneNumber { get; set; }
    [Required] [MinLength(2)] public string Citizenship { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required] public DateOnly DateOfBirth { get; set; }
    [Required] [MinLength(5)] public string Password { get; set; }
}