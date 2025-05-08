using System.ComponentModel.DataAnnotations;

namespace Cw_7_s30338.Models.DTOs;

public class ClientCreateDTO
{
    [MaxLength(120)]
    public required string FirstName { get; set; }
    [MaxLength(120)]
    public required string LastName { get; set; }
    [MaxLength(120)][RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    //poczatek -> same litery i cyfry, potem @, potem znow same litery i cyfry, potem . nastepnie litery i cyfry
    public required string Email { get; set; }
    [MaxLength(120)][RegularExpression("[0-9]")]
    public required string Telephone { get; set; }
    [MaxLength(120)][RegularExpression("[0-9]")]
    public required string Pesel { get; set; }
}