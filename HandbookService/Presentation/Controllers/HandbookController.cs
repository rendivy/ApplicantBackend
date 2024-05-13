using HandbookService.Domain.Model.Education;
using HandbookService.Domain.Service;
using Microsoft.AspNetCore.Mvc;

namespace HandbookService.Presentation.Controllers;

[ApiController]
[Route("api/handbook")]
public class HandbookController(IHandbookService handbookService) : Controller
{
    [HttpPost]
    [Route("update-faculty")]
    public async Task<IActionResult> UpdateFaculty()
    {
        await handbookService.ImportAllHandbookDataAsync();
        return Ok();
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetProgramById(Guid id)
    {
        EducationProgram? program = await handbookService.GetProgramByIdAsync(id);
        return Ok(program);
    }
}