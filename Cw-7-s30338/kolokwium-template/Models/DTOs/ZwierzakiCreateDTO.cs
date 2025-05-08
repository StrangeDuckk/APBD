using System.ComponentModel.DataAnnotations;

namespace Cw_7_s30338.Models.DTOs;

public class ZwierzakiCreateDTO
{
    [Length(3,30)]
    public required string NazwaGatunku { get; set; }
    [MaxLength(50)]
    public required string Imie { get; set; }
    
    //
    // [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")] //regular expresion dla maila
    
}