using EnrollmentService.Application.Model;
using EnrollmentService.Application.Services;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
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
    public async Task<IActionResult> EnrollStudent(AdmissionRequest request)
    {
        await admissionService.CreateAdmission(request, Guid.NewGuid());
        return Ok();
    }
}