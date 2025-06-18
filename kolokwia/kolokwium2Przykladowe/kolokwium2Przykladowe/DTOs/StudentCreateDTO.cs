namespace kolokwium2Przykladowe.DTOs;

public class StudentCreateDTO
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
}