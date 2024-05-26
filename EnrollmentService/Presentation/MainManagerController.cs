using System.Security.Claims;
using EnrollmentService.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentService.Presentation;

[ApiController]
public class MainManagerController(IMainManagerService managerService) : Controller
{
    [HttpGet]
    [Route("manager-list")]
    [Authorize]
    public async Task<IActionResult> GetManagers()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        var managers = await managerService.GetManagers(userId!, userRole!);
        return Ok(managers);
    }


    [HttpPost]
    [Route("set-manager-on-enrollment")]
    [Authorize]
    public async Task<IActionResult> SetManagerOnEnrollment(string managerId, string enrollmentId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        await managerService.SetManagerOnEnrollment(userId!, userRole!, managerId, enrollmentId);
        return Ok();
    }
}