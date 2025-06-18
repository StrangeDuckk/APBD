namespace kolokwium2Przykladowe.DTOs;

public class CourseWithEnrollmentsGetDTO
{
    public string message {get; set;} = null!;
    public CourseGetDTO Course {get; set;} = null!;
    public IEnumerable<EnrollmentOnlyStudentGetDTO> Enrollments {get; set;} = null!;
}