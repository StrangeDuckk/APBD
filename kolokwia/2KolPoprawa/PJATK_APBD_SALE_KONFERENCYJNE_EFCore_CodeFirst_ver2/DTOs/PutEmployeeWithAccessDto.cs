using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.DTOs;

public class PutEmployeeWithAccessDto
{
    [MaxLength(150)]
    public required string Name { get; set; }
    [MaxLength(150)]
    public required string Email { get; set; }
    public List<int>? AccessibleRoomsIds { get; set; }
}