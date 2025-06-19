using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

public class CreateStudentWithEnrollmentDto
{
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public required string LastName { get; set; }
    [MaxLength(150)]
    public string? Email { get; set; }
    public ICollection<CreateCoursesDto>? Courses { get; set; }
}