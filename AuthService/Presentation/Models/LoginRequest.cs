using System.ComponentModel.DataAnnotations;
using AuthService.Domain.Entity;

namespace AuthService.Presentation.Models;

public class LoginRequest
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] [MinLength(5)] public string Password { get; set; }
}