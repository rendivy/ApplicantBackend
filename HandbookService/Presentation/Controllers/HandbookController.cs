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
}