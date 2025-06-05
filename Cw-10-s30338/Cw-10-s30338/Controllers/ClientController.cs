using System.Data.Common;
using Cw_10_s30338.Exceptions;
using Cw_10_s30338.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotFound = Cw_10_s30338.Exceptions.NotFound;

namespace Cw_10_s30338.Controllers;

[Route("api/clients")]
public class ClientController(IDbService dbservice):ControllerBase
{
    [HttpDelete]
    [Route("/{idClient}")]
    public async Task<IActionResult> DeleteClient([FromRoute] string idClient)
    {
        try
        {
            await dbservice.DeleteClientAsync(idClient);
            return Ok(new
                { message = $"Client {idClient} succesfully deleted" }); //nie wiem jaki powinien byc kod bledu
        }
        catch (NotFound ex)
        {
            return NotFound(ex.Message);
        }
        catch (DeletingClientWithTripsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}