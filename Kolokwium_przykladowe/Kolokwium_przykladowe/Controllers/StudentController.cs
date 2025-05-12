using Kolokwium_przykladowe.Models;
using Kolokwium_przykladowe.Models.DTOs;
using Kolokwium_przykladowe.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium_przykladowe.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(IDbService service):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStudent([FromQuery] string? firstName)
    {
        try
        {
            return Ok(await service.GetStudentsWithGroupsAsync(firstName));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDTO body)
    {
        try
        {
            var student = await service.CreateStudentAsync(body);
            return Created($"students/{student.Id}", student);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] StudentUpdateDTO body)
    {
        try
        {
            await service.UpdateStudentAsync(id, body);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}