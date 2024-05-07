using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Models;
using AuthService.Presentation.Models.Token;
using Common.Exception;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Services;

public class TokenService(
    JwtProvider jwtProvider,
    RedisDatabaseContext redisDatabaseContext,
    UserManager<User> userManager)
    : ITokenService
{
    public async Task<TokenResponse> GetNewPairOfTokens(string refreshToken)
    {
        var userId = await redisDatabaseContext.GetUserIdByRefreshToken(refreshToken);
        if (userId == null) throw new InvalidRefreshTokenException();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) throw new UserNotFoundException("User not found");
        var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var response = jwtProvider.CreateTokenResponse(new Guid(user.Id), userRole);
        await redisDatabaseContext.AddRefreshToken(response.RefreshToken, userId);
        await redisDatabaseContext.DeleteRefreshToken(refreshToken);
        return response;
    }
}