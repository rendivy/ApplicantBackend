using AuthService.Presentation.Models;
using AuthService.Presentation.Models.Token;

namespace AuthService.Domain.Interfaces;

public interface ITokenService
{
    public Task<TokenResponse> GetNewPairOfTokens(string refreshToken);
}