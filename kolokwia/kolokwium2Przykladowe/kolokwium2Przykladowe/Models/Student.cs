using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium2Przykladowe.Models;

[Table("Student")]
public class Student
{
    [Column("ID")]
    [Key]
    public int Id { get; set; }
    
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [MaxLength(100)]
    public required string LastName { get; set; }
    
    [MaxLength(150)]
    public string? Email { get; set; }
    
    // ---------- wlasciwosci nawigacyjne -----------
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}