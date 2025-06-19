using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.DTOs;

public class GetEmployeeByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<GetRoomAccessDto> AccesibleRooms { get; set; } = null!;
}