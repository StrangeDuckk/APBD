using Cw_7_s30338.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Cw_7_s30338.Services;

namespace Cw_7_s30338.Controllers;

[ApiController]
[Route("[controller]")]
public class ZwierzakiController(IDbService service):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetZwierzaki()
    {
        return Ok(await service.GetAllZwierzakiAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetZwierzakiById([FromRoute] int id)
    {
        try
        {
            return Ok(await service.GetZwierzakiByIdAsync(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateZwierzak([FromBody] ZwierzakiCreateDTO body)
    {
        var animal = await service.CreateZwierzakiAsync(body);
        return CreatedAtAction(nameof(GetZwierzakiById), new {id = animal.Id_Zwierzaka}, animal);
    }
        
}