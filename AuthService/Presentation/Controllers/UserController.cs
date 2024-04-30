using System.Security.Claims;
using AuthService.Application.Services.Models.Profile;
using AuthService.Domain.Entity;
using AuthService.Domain.Interfaces;
using AuthService.Presentation.Models.Account;
using AuthService.Presentation.Models.Token;
using EasyNetQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(
    IAccountService accountService,
    ITokenService tokenService,
    IProfileService profileService,
    IBus bus
) : Controller
{
    [HttpGet]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<UserRequest> GetUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return await profileService.GetUserProfile(userId);
    }

    [HttpPut]
    [Route("profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task UpdateUserInfo(UpdateProfileRequest updateProfileRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await profileService.UpdateProfile(userId, updateProfileRequest);
    }

    
    [HttpPost]
    [Route("refresh-token")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<TokenResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest)
    {
        return await tokenService.GetNewPairOfTokens(refreshTokenRequest.RefreshToken);
    }


    [HttpPost]
    [Route("{userId}/role")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task AddRole(string userId, Roles role)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.Name);
        await accountService.AddRole(currentUserId, userId, role);
    }


    [HttpPost]
    [Route("login")]
    public async Task<TokenResponse> Login(LoginRequest loginRequest)
    {
        return await accountService.Login(loginRequest);
    }


    [HttpPost]
    [Route("registration")]
    public async Task<TokenResponse> Registration(RegistrationRequest registrationRequest)
    {
        return await accountService.Registration(registrationRequest);
    }
}