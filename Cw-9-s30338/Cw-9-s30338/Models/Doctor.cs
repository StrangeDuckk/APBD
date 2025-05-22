using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Doctor")]
[PrimaryKey(nameof(IdDoctor))]
public class Doctor
{
    [Key]
    public int IdDoctor { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    //nawigacyjne:
    public IEnumerable<Prescription> Prescriptions { get; set; } = null!;
}