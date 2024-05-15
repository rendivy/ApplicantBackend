using System.Security.Claims;
using EnrollmentService.Application.Model;
using EnrollmentService.Application.Services;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentService.Presentation;

[ApiController]
public class EnrollmentController : Controller
{
    private readonly IAdmissionService admissionService;

    public EnrollmentController(IAdmissionService admissionService)
    {
        this.admissionService = admissionService;
    }

    [HttpPost]
    [Route("api/enrollment")]
    [Authorize]
    public async Task<IActionResult> EnrollStudent(AdmissionRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await admissionService.CreateAdmission(request, new Guid(userId));
        return Ok();
    }
}