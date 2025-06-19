using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

[Table("StudentCourse")]
[PrimaryKey(nameof(StudentId),nameof(CourseId))]
public class StudentCourse
{
    [Column("Course_ID")]
    public int CourseId { get; set; }
    [Column("Student_ID")]
    public int StudentId { get; set; }
    
    // ---------- nawigacyjne ---------
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
}