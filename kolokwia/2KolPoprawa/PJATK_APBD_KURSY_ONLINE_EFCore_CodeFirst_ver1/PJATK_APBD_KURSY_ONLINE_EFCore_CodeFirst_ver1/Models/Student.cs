using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

[Table("Student")]
[PrimaryKey(nameof(StudentId))]
public class Student
{
    [Column("ID")]
    public int StudentId { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    [MaxLength(150)]
    public string? Email { get; set; }
    
    // -------- nawigacyjne --------
    public virtual ICollection<StudentCourse> StudentCourse { get; set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}