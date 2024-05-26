using System.Security.Claims;
using EnrollmentService.Domain.Entity;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentService.Presentation;

[ApiController]
public class ManagerController(IManagerService managerService, IDocumentService documentService) : Controller
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


    [HttpGet]
    [Route("get-applicant-document")]
    [Authorize]
    public async Task<IActionResult> GetApplicantDocument(string applicantId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        if (userRole != "Manager")
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        var documents = await managerService.GetApplicantDocuments(applicantId, userId!);
        return Ok(documents);
    }

    [HttpGet]
    [Route("get-applicant-passport")]
    [Authorize]
    public async Task<IActionResult> GetApplicantPassport(string applicantId)
    {
        var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        if (userRole != "Manager")
            throw new UnauthorizedAccessException("You are not authorized to get applicant documents");
        var passport = await documentService.GetPassportInformation(applicantId);
        return Ok(passport);
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


    [HttpGet]
    [Route("get-applicant-enrollment")]
    [Authorize]
    public async Task<IActionResult> GetApplicantEnrollment(string? name, string? program, Guid? faculties,
        EnrollmentStatus? status, bool unassigned, SortOrder sortOrder, int pageNumber = 1, int pageSize = 10)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var enrollments = await managerService.GetApplicantEnrollment(name, program, faculties, status, unassigned,
            userId!, pageNumber, pageSize, sortOrder);
        return Ok(enrollments);
    }
}