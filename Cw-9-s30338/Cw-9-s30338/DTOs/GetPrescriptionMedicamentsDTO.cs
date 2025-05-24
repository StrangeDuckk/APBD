using Cw_9_s30338.Models;

namespace Cw_9_s30338.DTOs;

public class GetPrescriptionMedicamentsDTO
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;

    //wlasciwosci nawigacyjne
    public Medicament Medicament { get; set; } = null!;
    public Prescription Prescription { get; set; } = null!;
}