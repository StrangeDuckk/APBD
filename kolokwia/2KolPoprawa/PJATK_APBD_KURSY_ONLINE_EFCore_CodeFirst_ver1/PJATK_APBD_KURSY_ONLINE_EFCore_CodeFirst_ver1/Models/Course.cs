using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

[Table("Course")]
[PrimaryKey(nameof(CourseId))]
public class Course
{
    [Column("ID")]
    public int CourseId { get; set; }    
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(50)]
    public string Category { get; set; } = null!;
    
    // ---------- nawigacyjne ------------
    public virtual ICollection<StudentCourse> StudentCourse { get; set; } = null!;
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}