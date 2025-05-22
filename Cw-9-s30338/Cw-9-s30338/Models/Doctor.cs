using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Doctor")]
[PrimaryKey(nameof(IdDoctor))]
public class Doctor
{
    [Key]
    private int IdDoctor { get; set; }
    [MaxLength(100)]
    private string FirstName { get; set; } = null!;
    [MaxLength(100)]
    private string LastName { get; set; } = null!;
    [MaxLength(100)]
    private string Email { get; set; } = null!;
    
    //nawigacyjne:
    private IEnumerable<Prescription> Prescription { get; set; } = null!;
}