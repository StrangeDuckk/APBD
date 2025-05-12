using System.ComponentModel.DataAnnotations;

namespace Kolokwium_przykladowe.Models.DTOs;

public class StudentCreateDTO
{
    [MaxLength(120)]//dla pewnosci
    public required string FirstName { get; set; }
    [MaxLength(120)]//dla pewnosci
    public required string LastName { get; set; }
    [Range(17,120)]//liczby wieksze niz 16 do 120 bo ile mozna zyc
    public required int Age { get; set; }
    public List<int>? groupAssigements { get; set; }
}