using Cw_10_s30338.DTOs;
using Cw_10_s30338.Exceptions;
using Cw_10_s30338.Models;
using Cw_10_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_10_s30338.Controllers;

[ApiController]
[Route("api/trips")]
public class ClientTripController(IDbService dbService): ControllerBase
{
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> PostClientWithTripAsync([FromRoute] int idTrip, [FromBody] CreateClientTripDTO clientTrip)
    {
        try
        {
            var created = await dbService.AddClietnTripAsync(clientTrip);
            return CreatedAtAction(nameof(GetClientTripDTO), new { name = created.client.FirstName }, created);
        }
        catch (NotFound ex)
        {
            throw new NotFound(ex.Message);
        }
        catch (AlreadyExistingClientException ex)
        {
            return Conflict(ex.Message);
        }
        catch (TripAlreadyStartedException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}