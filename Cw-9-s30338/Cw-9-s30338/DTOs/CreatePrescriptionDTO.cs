using Cw_9_s30338.Models;

namespace Cw_9_s30338.DTOs;

public class CreatePrescriptionDTO
{
    public required Patient CreatePatient { get; set; } = null!;
    public required Doctor Doctor { get; set; } = null!;
    public required List<Medicament> Medicament { get; set; } = null!;
    public required DateTime Date { get; set; }
    public required DateTime DueDate { get; set; }
}