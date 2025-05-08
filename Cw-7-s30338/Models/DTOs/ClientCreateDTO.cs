using System.ComponentModel.DataAnnotations;

namespace Cw_7_s30338.Models.DTOs;

public class ClientCreateDTO
{
    [MaxLength(120)]
    public required string FirstName { get; set; }
    [MaxLength(120)]
    public required string LastName { get; set; }
    [MaxLength(120)]
    public required string Email { get; set; }
    [MaxLength(120)]
    public required string Telephone { get; set; }
    [MaxLength(120)]
    public required string Pesel { get; set; }
}