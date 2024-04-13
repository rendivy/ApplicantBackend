using System.Security.Claims;
using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IAccountService accountService) : Controller
{
    [HttpGet]
    [Route("info")]
    [Authorize]
    public async Task<UserRequest> GetUserInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        return await accountService.GetUserById(userId);
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