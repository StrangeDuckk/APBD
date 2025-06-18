using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;

[Table("Student")]
[PrimaryKey(nameof(StudentId))]
public class Student
{
    [Column("ID")]
    public int StudentId { get; set; }
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [MaxLength(150)]
    public string? Email { get; set; }
    
    // ------------ nawigacyjne --------------------
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}