namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

public class StudentGetDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime EnrollmentDate { get; set; }
}