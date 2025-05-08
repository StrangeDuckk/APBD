using Cw_7_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_7_s30338.Controllers;

[ApiController]
[Route("clients")]
public class ClientController(IDbService service):ControllerBase
{
    //-------- pobieranie wszystkich wycieczek powiazanych z klientem -------
    [HttpGet]
    [Route("{id}/trips")]
    public async Task<IActionResult> GetClientTrips(int id)
    {
        try
        {
            return Ok(await service.GetClientsTrips(id));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}