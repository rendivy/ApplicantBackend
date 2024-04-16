using AuthService.Domain.Entity;
using AuthService.Domain.Exception;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data.Database;
using AuthService.Presentation.Models;
using AuthService.Presentation.Models.Token;
using Common.CustomException;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Services;

public class TokenService(JwtProvider jwtProvider, AuthDbContext authDbContext, UserManager<User> userManager)
    : ITokenService
{
    public async Task<TokenResponse> GetNewPairOfTokens(string refreshToken)
    {
        var userRefreshToken = await authDbContext.UserRefreshTokens
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        if (userRefreshToken == null) throw new InvalidRefreshTokenException();

        var user = await userManager.FindByIdAsync(userRefreshToken.UserId);

        if (user == null) throw new UserNotFoundException("User not found");

        var userRole = (await userManager.GetRolesAsync(user)).FirstOrDefault();
        var response = jwtProvider.CreateTokenResponse(new Guid(user.Id), userRole);
        authDbContext.UserRefreshTokens.Remove(userRefreshToken);
        await authDbContext.AddAsync(new UserRefreshTokens
        {
            UserId = user.Id,
            RefreshToken = response.RefreshToken
        });
        await authDbContext.SaveChangesAsync();
        return response;
    }
}