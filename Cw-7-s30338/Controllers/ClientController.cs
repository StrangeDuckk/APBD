using Cw_7_s30338.Exceptions;
using Cw_7_s30338.Models.DTOs;
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

    [HttpPost]
    public async Task<IActionResult> PostClient([FromBody] ClientCreateDTO body)
    {
        var client = await service.CreateClient(body);
        return CreatedAtAction(nameof(GetClientById), new {IdClient = client.Id}, client);
    }

    [HttpGet]
    [Route("{id}")]
    private async Task<IActionResult> GetClientById(int id)
    {
        try
        {
            return Ok(await service.GetClientById(id));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    [Route("{id}/trips/{tripId}")]
    public async Task<IActionResult> PutClientTrip(int id, int tripId)
    {
        try
        {
            var clientTrip = await service.putClientTrip(id, tripId);
            return NoContent();//no content
        }
        catch (FilledTripException e)
        {
            return BadRequest(e.Message);//blad 400
        }
        catch (Exception e)
        {
            return NotFound(e.Message);//404
        }
    }
    [HttpDelete]
    [Route("{id}/trips/{tripId}")]
    public async Task<IActionResult> DeleteClientTrip(int id, int tripId)
    {
        try
        {
            await service.DeleteClientTrip(id, tripId);
            return NoContent();//no content
        }
        catch (Exception e)
        {
            return NotFound(e.Message);//404
        }
    }
}