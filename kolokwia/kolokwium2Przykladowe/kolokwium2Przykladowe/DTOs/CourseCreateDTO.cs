namespace kolokwium2Przykladowe.DTOs;

public class CourseCreateDTO
{
    public CourseGetDTO Course { get; set; } = null!;
    public IEnumerable<StudentGetDTO> Students { get; set; } = null!;
}