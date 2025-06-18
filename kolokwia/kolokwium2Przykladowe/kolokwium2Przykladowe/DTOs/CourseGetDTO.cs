namespace kolokwium2Przykladowe.DTOs;

public class CourseGetDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Credits { get; set; }
    public string Teacher { get; set; } = null!;
}