using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entity;

public class UserRefreshTokens
{
    [Key]
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
}