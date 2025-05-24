using Cw_9_s30338.Models;

namespace Cw_9_s30338.DTOs;

public class GetMedicamentDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    //gdyby medykamenty mialy wiele czegos dodajemy na dole liste tych rzeczy:
    public IEnumerable<Prescription_Medicament> Prescription_Medicaments { get; set; }
}