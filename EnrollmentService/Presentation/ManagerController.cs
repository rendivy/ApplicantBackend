using System.Security.Claims;
using EnrollmentService.Domain.Entity;
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

    [HttpPost]
    [Route("remove-manager-from-enrollment")]
    [Authorize]
    public async Task<IActionResult> RemoveManagerFromEnrollment(string enrollmentId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await managerService.RemoveManagerFromEnrollment(enrollmentId, userId!);
        return Ok();
    }

    [HttpGet]
    [Route("get-applicant-documents")]
    [Authorize]
    public async Task<IActionResult> GetApplicantDocuments(string applicantId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var documents = await managerService.GetApplicantDocuments(applicantId, userId!);
        return Ok(documents);
    }

    [HttpPost]
    [Route("set-applicant-enrollment-status")]
    [Authorize]
    public async Task<IActionResult> SetApplicantEnrollmentStatus(string enrollmentId, EnrollmentStatus status,
        string message)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await managerService.SetApplicantEnrollmentStatus(enrollmentId, status, message, userId!);
        return Ok();
    }
}