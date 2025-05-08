using Cw_7_s30338.Models.DTOs;
using Cw_7_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_7_s30338.Controllers;

[ApiController]
[Route("trips")]
public class TripController(IDbService service):ControllerBase
{
    // ----------- pobranie wszystkich informacji o dostepnych wycieczkach wraz z krajami -------
    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        try
        {
            return Ok(await service.GetTripsInfoAndCountries());
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}