using System.Security.Claims;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entity;
using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IAccountService accountService) : Controller
{
    [HttpGet]
    [Route("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<UserRequest> GetUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return await accountService.GetUserById(userId);
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