using System.Security.Claims;
using AuthService.Application.Services;
using AuthService.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(AccountService accountService) : Controller
{
    [HttpGet]
    [Route("info")]
    public async Task<UserDTO> GetUserInfo()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
        return await accountService.GetUserById(userId);
    }
}