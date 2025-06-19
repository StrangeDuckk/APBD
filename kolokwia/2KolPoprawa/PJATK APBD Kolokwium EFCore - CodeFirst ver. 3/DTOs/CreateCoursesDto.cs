using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

public class CreateCoursesDto
{
    [MaxLength(150)]
    public required string Title { get; set; }
    [MaxLength(300)]
    public string? Credits { get; set; }
    [MaxLength(150)]
    public required string Teacher { get; set; }
}