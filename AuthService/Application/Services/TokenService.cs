using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services;

public class TokenService(JwtProvider jwtProvider) : ITokenService
{
    public Task<string> GetNewPairOfTokens(string refreshToken)
    {
        throw new NotImplementedException();
    }
}