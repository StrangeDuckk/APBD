namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.DTOs;

public class GetEnrollmentsDto
{
    public GetStudentDto Student { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
    public string CompletionStatus { get; set; } = null!;
}