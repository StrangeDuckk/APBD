using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kolokwium2Przykladowe.Models;

[Table("Enrollment")]
[PrimaryKey(nameof(Student_Id), nameof(Course_Id))]
public class Enrollment
{
    [Column("Student_ID")]
    public int Student_Id { get; set; } 
    
    [Column("Course_ID")]
    public int Course_Id { get; set; }
    
    public DateTime EnrollmentDate { get; set; }
    
    // ---------- wlasciwosci nawigacyjne -----------
    [ForeignKey(nameof(Student_Id))]
    public virtual Student Student { get; set; } = null!;
    [ForeignKey(nameof(Course_Id))]
    public virtual Course Course { get; set; } = null!;
}