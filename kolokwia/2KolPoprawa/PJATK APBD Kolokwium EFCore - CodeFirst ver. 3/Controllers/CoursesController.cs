using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Services;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(IDbService service):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCoursesAsync()
    {
        return Ok(await service.GetAllCoutsesAsync());
    }
}