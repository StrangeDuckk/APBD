using Cw_9_s30338.Models;

namespace Cw_9_s30338.DTOs;

public class GetPatientDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    
    //nawigacyjne:
    public IEnumerable<Prescription> Prescriptions { get; set; } = null!;
}