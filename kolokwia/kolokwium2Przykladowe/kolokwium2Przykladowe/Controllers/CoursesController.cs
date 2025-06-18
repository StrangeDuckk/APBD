using kolokwium2Przykladowe.DTOs;
using kolokwium2Przykladowe.Exceptions;
using kolokwium2Przykladowe.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium2Przykladowe.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(IDbService service):ControllerBase
{
    [Route("with-enrollments")]
    [HttpPost]
    public async Task<IActionResult> CreateCourseFromBodyAsync([FromBody] CourseCreateDTO courseCreateDTO)
    {
        try
        {
            var created = await service.AddNewCourseWithStudentsAsync(courseCreateDTO);
            return CreatedAtAction(nameof(CourseCreateDTO), new{id = created.Course.Id}, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}