namespace kolokwium2Przykladowe.DTOs;

public class EnrollmentOnlyStudentGetDTO
{
    public DateTime EnrollmentDate { get; set; }
    
    // ---------- wlasciwosci nawigacyjne -----------
    public StudentGetDTO StudentGetDTO { get; set; } = null!;
}