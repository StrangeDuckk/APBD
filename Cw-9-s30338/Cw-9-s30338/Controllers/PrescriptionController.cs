using Cw_9_s30338.DTOs;
using Cw_9_s30338.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw_9_s30338.Controllers;

[ApiController]
[Route("prescription")]
public class PrescriptionController(IDbService service) :ControllerBase
{
    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddNewPrescriptionMedicamentAsync(
        [FromBody] CreatePrescriptionDTO prescriptionDTO)
    {
        try
        {
            var created = await service.AddNewPrescriptionAsync(prescriptionDTO);
            return CreatedAtAction(nameof(GetPrescriptionDTO), new{id = created.Patient.IdPatient}, created);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}