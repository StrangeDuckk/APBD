using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.DTOs;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Services;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Controllers;
[ApiController]
[Route("api/employees")]
public class EmployeesController(IDbService service):ControllerBase
{
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetEmployee([FromRoute] int id)
    {
        var result = await service.GetEmployeeByIdAsync(id);
        if (result == null)
            return NotFound($"Employee with id {id} not found");
        
        return Ok(result);
    }

    [HttpPut]
    [Route("{id:int}/with-access")]
    public async Task<IActionResult> PutEmployeeWithAccess([FromRoute] int id,
        [FromBody] PutEmployeeWithAccessDto employee)
    {
        var result = await service.PutEmployeeWithAccessAsync(id, employee);
        if(result == null)
            return NotFound($"Employee with id {id} not found");
        return Ok(result);
    }
}