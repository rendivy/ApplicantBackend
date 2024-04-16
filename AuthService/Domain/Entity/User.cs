using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entity;

public class User : IdentityUser
{
    [Required] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Citizenship { get; set; }
}

