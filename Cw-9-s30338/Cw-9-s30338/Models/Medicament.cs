using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Medicament")]
[PrimaryKey(nameof(IdMedicament))]//po przecinku mozna zrobic zlozony
public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [MaxLength(100)] 
    public string Description { get; set; } = null!;
    [MaxLength(100)] 
    public string Type { get; set; } = null!;
    //gdyby medykamenty mialy wiele czegos dodajemy na dole liste tych rzeczy:
    public IEnumerable<Prescription_Medicament> Prescription_Medicaments { get; set; } = null!;
}