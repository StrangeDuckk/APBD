using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Patient")]
[PrimaryKey(nameof(IdPatient))]
public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    
    //nawigacyjne:
    public IEnumerable<Prescription> Prescriptions { get; set; } = null!;
}