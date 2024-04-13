namespace AuthService.Presentation.Models;

public record UserRequest
{
    public string Id { get; init; }
    public string Email { get; init; }
    
}
