using Cw_10_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_10_s30338.Controllers;

[Route("api/trips")]
public class TripController(IDbService dbService):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTripsDescByStartAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        try
        {
            return Ok(await dbService.GetAllTripsAsync(page,pageSize));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}