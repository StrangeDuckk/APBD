using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

[Table("Enrollment")]
[PrimaryKey(nameof(StudentId),nameof(CourseId))]
public class Enrollment
{
    [Column("Course_ID")]
    public int CourseId { get; set; }
    [Column("Student_ID")]
    public int StudentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    [MaxLength(50)]
    public string CompletionStatus { get; set; } = null!;
    
    // ---------- nawigacyjne -----------
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;
    [ForeignKey(nameof(CourseId))]
    public virtual Course Course { get; set; } = null!;
}