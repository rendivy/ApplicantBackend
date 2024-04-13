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
    public async Task<UserDTO> GetUserInfo()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return await accountService.GetUserById(userId);
    }


    [HttpPost]
    [Route("registration")]
    public async Task<TokenResponse> Registration(RegistrationRequest registrationRequest)
    {
        return await accountService.Registration(registrationRequest);
    }
}