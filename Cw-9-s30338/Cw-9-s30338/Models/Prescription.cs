﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cw_9_s30338.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Models;

[Table("Prescription")]
[PrimaryKey(nameof(IdPrescription))]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    //nie uzywac forein key zaznaczonego
    public int IdPatient { get; set; }
    public Patient Patient { get; set; } = null!;//dodanie wlasciwosci nawigacyjnych
    
    public int IdDoctor { get; set; }
    public Doctor Doctor { get; set; } = null!;//wlasciwosci nawigacyjne
    
    //nawigacyjne do tabeli posredniej
    public List<Prescription_Medicament> PrescriptionMedicaments { get; set; }
}