using System.Security.Claims;
using EnrollmentService.Domain.Service;
using EnrollmentService.Presentation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnrollmentService.Presentation;

[ApiController]
public class EnrollmentController(IAdmissionService admissionService, IDocumentService documentService)
    : Controller
{
    [HttpPost("upload-scan")]
    [Authorize]
    public async Task<IActionResult> Upload(IFormFile file, string educationDocumentId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await documentService.SaveDocumentScan(file, userId!, educationDocumentId);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    [Route("get-passport")]
    public async Task<PassportDocumentResponse> GetPassportInformation()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var result = await documentService.GetPassportInformation(userId!);
        return result;
    }

    [HttpPost("upload-education-document")]
    [Authorize]
    public async Task<IActionResult> Upload(EducationDocumentRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await documentService.AddEducationDocumentInformation(request, userId!);
        return Ok();
    }

    [HttpPut("edit-education-document")]
    [Authorize]
    public async Task<IActionResult> EditEducationDocument(EducationDocumentRequest request, string educationDocumentId)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await documentService.EditDocument(request, educationDocumentId, userId!);
        return Ok();
    }

    [HttpGet("get-education-document")]
    [Authorize]
    public async Task<List<EducationDocumentResponse>> GetEducationDocumentInformation()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        var result = await documentService.GetEducationDocumentInformation(userId!);
        return result;
    }


    [HttpPost("upload-passport")]
    [Authorize]
    public async Task<IActionResult> Upload(PassportDocumentRequest passportDocumentRequest)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await documentService.AddPassportInformation(passportDocumentRequest, userId!);
        return Ok();
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

    [HttpPut]
    [Route("api/enrollment")]
    [Authorize]
    public async Task<IActionResult> EditEnrollment(AdmissionRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
        await admissionService.EditAdmission(request, new Guid(userId));
        return Ok();
    }
}