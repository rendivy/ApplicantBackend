using System.ComponentModel.DataAnnotations;

namespace AuthService.Presentation.Models.Account;

public class LoginRequest
{
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] [MinLength(5)] public string Password { get; set; }
}