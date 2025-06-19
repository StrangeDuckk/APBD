using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Services;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController(IDbService service):ControllerBase
{
    [HttpPost]
    [Route("with-enrollments")]
    public async Task<IActionResult> PostStudentWithEnrollmentsAsync(CreateStudentWithEnrollmentDto studentwithEnrollments)
    {
        return Ok(await service.PostStudentWithEnrollmentsAsync(studentwithEnrollments));
    }
}