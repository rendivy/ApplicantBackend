using System.Security.Claims;
using EnrollmentService.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentService.Presentation;

[ApiController]
public class ManagerController(IManagerService managerService) : Controller
{
    [HttpPost]
    [Route("set-manager-to-enrollment")]
    [Authorize]
    public async Task<IActionResult> SetManagerOnEnrollment(string enrollmentId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await managerService.SetManagerOnEnrollment(enrollmentId, userId!);
        return Ok();
    }
}