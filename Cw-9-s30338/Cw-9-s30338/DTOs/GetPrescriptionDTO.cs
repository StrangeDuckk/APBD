using Cw_9_s30338.Models;

namespace Cw_9_s30338.DTOs;

public class GetPrescriptionDTO
{
    public Patient Patient { get; set; }
    public Doctor Doctor { get; set; }
    
    public DateTime Date { get; set; }
    public DateTime DueDate {get; set; }
    public List<GetPrescriptionMedicamentsDTO> PrescriptionMedicaments { get; set; }
}