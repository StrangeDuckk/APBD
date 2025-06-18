using kolokwium2Przykladowe.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium2Przykladowe.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EntrollmentController(IDbService service): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> EnrollmentGetAsync()
    {
        try
        {
            return Ok(await service.EnrollmentGetAllAsync());
        }
        catch (Exception ex)
        {
            return NotFound(ex);
        }
    }
}