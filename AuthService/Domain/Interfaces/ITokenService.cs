namespace AuthService.Domain.Interfaces;

public interface ITokenService
{
    public Task<string> GetNewPairOfTokens(string refreshToken);
}