using Cw_9_s30338.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Data;

public class AppDbContext :DbContext
{
    public DbSet<Medicament> Medicaments { get; set; }//operacje na tym jak na rzeczywistej tabeli
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription_Medicament> PrescriptionMedicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Prescription_Medicament>().HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });//dla kluczy zlozonych
        //ustawienia forein key w relacji 1:N, dla kazdego forein oddzielnie
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions) //tablica wiele z modelu doctor
            .HasForeignKey(p => p.IdDoctor);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(d => d.Prescriptions) //tablica wiele z modelu patient
            .HasForeignKey(p => p.IdPatient);
        
        var medicament = new Medicament
        {
            IdMedicament=1,
            Name="Viagra",
            Description="makes you unstoppable, do not overdose or you will stop :)",
            Type = "without a receipt"
        };

        var doctor = new Doctor
        {
            IdDoctor = 1,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "Jan.kowalski@znanyDoktor.pl"
        };

        var patient = new Patient
        {
            IdPatient = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date=new DateTime(2010, 1, 1),
            DueDate=new DateTime(2020, 10, 1),
            IdPatient = 1,
            IdDoctor = 1
            //nie uzywac Doctor = doctor gdzie doctor jest napisany wyzej
        };

        var prescMedi = new Prescription_Medicament
        {
            IdMedicament = 1,
            IdPrescription = 1,
            Dose = 15,
            Details = "one pill every two days"
        };
        
        modelBuilder.Entity<Medicament>().HasData(medicament);
        modelBuilder.Entity<Doctor>().HasData(doctor);
        modelBuilder.Entity<Patient>().HasData(patient);
        modelBuilder.Entity<Prescription>().HasData(prescription);
        modelBuilder.Entity<Prescription_Medicament>().HasData(prescMedi);
    }
}