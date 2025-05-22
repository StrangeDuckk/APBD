using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Prescription_Medicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class Prescription_Medicament
{
    [ForeignKey(nameof(IdMedicament))]
    [Key]
    public int IdMedicament { get; set; }
    
    [ForeignKey(nameof(IdPrescription))]
    [Key]
    public int IdPrescription { get; set; }
    
    public int? Dose { get; set; }
    [MaxLength(100)]
    public string Details { get; set; } = null!;

    //wlasciwosci nawigacyjne
    public Medicament Medicament { get; set; } = null!;
    public Prescription Prescription { get; set; } = null!;
}