namespace kolokwium2Przykladowe.DTOs;

public class EnrollmentGetDTO
{
    public DateTime EnrollmentDate { get; set; }
    
    // ---------- wlasciwosci nawigacyjne -----------
    public StudentGetDTO StudentGetDTO { get; set; } = null!;
    public CourseGetDTO CourseGetDTO { get; set; } = null!;
}