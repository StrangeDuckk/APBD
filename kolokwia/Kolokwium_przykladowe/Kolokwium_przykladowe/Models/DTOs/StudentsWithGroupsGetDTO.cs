namespace Kolokwium_przykladowe.Models.DTOs;

public class StudentsWithGroupsGetDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public List<GroupsGetDTO> Groups { get; set; }
}