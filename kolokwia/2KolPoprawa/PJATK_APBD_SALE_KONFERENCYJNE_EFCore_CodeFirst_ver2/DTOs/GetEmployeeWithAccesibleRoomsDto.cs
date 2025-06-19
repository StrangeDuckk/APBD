namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.DTOs;

public class GetEmployeeWithAccesibleRoomsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<GetRoomAccessDto>? Rooms { get; set; }
    public string Message {get; set;} = null!;
}