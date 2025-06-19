namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

public class StudentWithEnrollmentsGetDto
{
    public StudentGetDto Student { get; set; } = null!;
    public ICollection<GetCoursesDetailsDto> Courses { get; set; } = null!;
}