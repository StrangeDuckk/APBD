using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;

[Table("Course")]
[PrimaryKey(nameof(CourseId))]
public class Course
{
    [Column("ID")]
    public int CourseId { get; set; }
    
    [MaxLength(150)]
    public string Title { get; set; } = null!;
    
    [MaxLength(300)]
    public string? Credits { get; set; }
    
    [MaxLength(150)]
    public string Teacher { get; set; } = null!;
    
    // ------------ nawigacyjne ------------
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}