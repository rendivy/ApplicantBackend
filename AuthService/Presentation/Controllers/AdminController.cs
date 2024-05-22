using System.Security.Claims;
using AuthService.Domain.Interfaces;
using AuthService.Presentation.Models.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(IAdminService adminService) : Controller
{
    [HttpPost]
    [Route("create-manager")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateManager(CreateManagerRequest createManagerRequest)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        await adminService.CreateManager(createManagerRequest, userRole!);
        return Ok();
    }
}