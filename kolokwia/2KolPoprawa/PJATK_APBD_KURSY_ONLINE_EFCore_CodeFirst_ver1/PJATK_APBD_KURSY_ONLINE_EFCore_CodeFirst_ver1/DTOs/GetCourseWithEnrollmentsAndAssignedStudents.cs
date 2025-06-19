using PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.DTOs;

public class GetCourseWithEnrollmentsAndAssignedStudents
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Category { get; set; } = null!;
    public ICollection<GetEnrollmentsDto> Enrollments { get; set; } = null!;
    public ICollection<GetStudentDto> AssignedStudents { get; set; } = null!;
}