using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Entity;
//TODO подумать над identity 
public class User : IdentityUser
{
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}