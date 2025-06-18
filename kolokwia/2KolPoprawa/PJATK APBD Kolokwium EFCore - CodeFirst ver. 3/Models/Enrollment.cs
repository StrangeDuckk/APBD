using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;

[Table("Enrollment")]
[PrimaryKey(nameof(StudentId), nameof(CourseId))]
public class Enrollment
{
    [Column("Student_ID")]
    public int StudentId { get; set; }
    
    [Column("Course_ID")]
    public int CourseId { get; set; }
    
    public DateTime EnrollmentDate { get; set; }
    // ---------- nawigacyjne -----------
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
}