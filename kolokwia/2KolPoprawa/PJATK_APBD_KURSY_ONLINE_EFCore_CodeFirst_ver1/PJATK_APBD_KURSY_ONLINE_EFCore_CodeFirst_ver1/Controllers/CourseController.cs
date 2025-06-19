using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Services;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController(IDbService service):ControllerBase
{
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCoursesById([FromRoute] int id)
    {
        var result = await service.GetCoursesByIdAsync(id);
        
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteCourseById([FromRoute] int id)
    {
        var result = await service.DeleteCourseByIdAsync(id);
        
        if (result == null)
            return NotFound();
        
        return Ok(new {message = result});
    }
}