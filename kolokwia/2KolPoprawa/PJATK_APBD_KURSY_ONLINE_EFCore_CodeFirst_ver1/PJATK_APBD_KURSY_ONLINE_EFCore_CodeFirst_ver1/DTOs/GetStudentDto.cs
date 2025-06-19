namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.DTOs;

public class GetStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; } = null!;
}