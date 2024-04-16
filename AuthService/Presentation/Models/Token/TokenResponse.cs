namespace AuthService.Presentation.Models.Token;

public class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}