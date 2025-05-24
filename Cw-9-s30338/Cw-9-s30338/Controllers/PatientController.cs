using Cw_9_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_9_s30338.Controllers;

[ApiController]
[Route("patient")]
public class PatientController(IDbService service):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatientAsync([FromQuery] int? id)
    {
        try
        {
            if (id.HasValue)
                return Ok(await service.GetPatientByIdAsync(id.Value));

            return Ok(await service.GetPatientsAsync());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}