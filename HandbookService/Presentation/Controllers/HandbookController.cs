using System.Security.Claims;
using Common.Exception;
using HandbookService.Domain.Model;
using HandbookService.Domain.Model.Education;
using HandbookService.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HandbookService.Presentation.Controllers;

[ApiController]
[Route("api/handbook")]
public class HandbookController(IHandbookService handbookService) : Controller
{
    [HttpPost]
    [Route("update-faculty")]
    [Authorize]
    public async Task<IActionResult> UpdateFaculty()
    {
        var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
        if (userRole != "Admin")
        {
            throw new UnauthorizedRoleException("Only admin can update faculty");
        }
        await handbookService.ImportAllHandbookDataAsync();
        return Ok();
    }

    [HttpGet]
    [Route("programs")]
    [Authorize]
    public async Task<IActionResult> GetPrograms(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromQuery] Guid? facultyId = null,
        [FromQuery] int? educationLevelId = null,
        [FromQuery] string educationForm = null,
        [FromQuery] string language = null,
        [FromQuery] string searchTerm = null)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0");
        }
        PagedResult<EducationProgram> programs = await handbookService.GetProgramsAsync(
            pageNumber,
            pageSize,
            facultyId,
            educationLevelId,
            educationForm,
            language,
            searchTerm);
        return Ok(programs);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProgramById(Guid id)
    {
        EducationProgram? program = await handbookService.GetProgramByIdAsync(id);
        return Ok(program);
    }
}