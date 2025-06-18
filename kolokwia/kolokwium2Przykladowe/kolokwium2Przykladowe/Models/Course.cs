using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kolokwium2Przykladowe.Models;

[Table("Course")]
public class Course
{
    [Column("ID")]
    [Key]
    public int Id { get; set; }

    [MaxLength(150)]
    public required string Title { get; set; }
    
    [MaxLength(300)]
    public string? Credits { get; set; }
    
    [MaxLength(150)]
    public required string Teacher { get; set; }
    
    // ---------- wlasciwosci nawigacyjne -----------
    public virtual ICollection<Enrollment> Enrollments { get; set; } = null!;
}