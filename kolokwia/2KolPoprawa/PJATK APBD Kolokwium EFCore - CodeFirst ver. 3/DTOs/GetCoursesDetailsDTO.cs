namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

public class GetCoursesDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Teacher { get; set; } = null!;
    public string? Credits { get; set; }
    public ICollection<StudentGetDto>? Student { get; set; }
}