using System.Data;
using Cw_9_s30338.Data;
using Cw_9_s30338.DTOs;
using Cw_9_s30338.Exceptions;
using Cw_9_s30338.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Services;

public interface IDbService
{
    Task<GetPrescriptionDTO> AddNewPrescriptionAsync(CreatePrescriptionDTO prescriptionDto);
}

public class DbService(AppDbContext data) :IDbService
{
    public async Task<GetPrescriptionDTO> AddNewPrescriptionAsync
        (CreatePrescriptionDTO prescriptionDto)
    {
        if (prescriptionDto == null)
        {
            throw new NoNullAllowedException("Cannot send empty prescription");
        }

        // ---------- sprawdzanie prawidlowej daty -------------
        if (prescriptionDto.DueDate < prescriptionDto.Date)
        {
            throw new DataException("Due date cannot be earlier than date itself");
        }

        // --------- sprawdzanie czy nie ma przekroczonej ilosci lekow ----------
        if (prescriptionDto.Medicament.Count > 10)
        {
            throw new TooManyMedicaments("Maximum medicaments count is 10");
        }
        
        
        // --------- sprawdzenie czy pacjent istnieje w bazie danych -----------
        if (prescriptionDto.CreatePatient != null) // w ciele mozna podac tylko jednego pacjenta
        {
            var patient =
                await data.Patients.FirstOrDefaultAsync(p =>
                    p.IdPatient == prescriptionDto.CreatePatient.IdPatient);
            if (patient is null)
            {
                await AddNewPatientAsync(prescriptionDto.CreatePatient);
            }
        }

        // ------- sprawdzenie czy lekarz istnieje w bazie danych --------
        if (prescriptionDto.Doctor != null)
        {
            var doctor =
                await data.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == prescriptionDto.Doctor.IdDoctor);
            if (doctor is null)
                throw new NotFoundException($"Doctor {prescriptionDto.Doctor.IdDoctor} not found");
        }

        // -------- sprawdzenie czy medicament istnieje w bazie ---------
        List<GetMedicamentDTO> Medicament = [];
        if (prescriptionDto.Medicament != null && prescriptionDto.Medicament.Count != 0)
        {
            foreach (var medicament in prescriptionDto.Medicament)
            {
                var medi = await data.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == medicament.IdMedicament
                );
                if (medi is null)
                {
                    throw new NotFoundException($"Medicament {medicament.IdMedicament} not found");
                }
            }
        }

        // uzycie zmiennej do zebrania danych -> podejscie 2, nie wymaga transakcji
        // ---------- zebranie danych --------------
        var presc = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdPatient = prescriptionDto.CreatePatient.IdPatient,
            Patient = prescriptionDto.CreatePatient,
            IdDoctor = prescriptionDto.Doctor.IdDoctor,
            Doctor = prescriptionDto.Doctor,
            PrescriptionMedicaments = new List<Prescription_Medicament>()
                .Select(pm => new Prescription_Medicament
                {
                    Details = pm.Details,
                    Dose = pm.Dose,
                    Medicament = pm.Medicament,
                    IdMedicament = pm.IdMedicament,
                    Prescription = pm.Prescription,
                    IdPrescription = pm.IdPrescription,
                }).ToList(),
        };

        // --------- dodanie ---------
        await data.Prescriptions.AddAsync(presc);
        await data.SaveChangesAsync();

        // --------- zwrot do controllera ------
        return new GetPrescriptionDTO
        {
            Patient = presc.Patient,
            Doctor = presc.Doctor,
            PrescriptionMedicaments = presc.PrescriptionMedicaments
                .Select(pm => new GetPrescriptionMedicamentsDTO
                {
                    IdMedicament = pm.IdMedicament,
                    IdPrescription = pm.IdPrescription,
                    Dose = pm.Dose,
                    Details = pm.Details,
                }).ToList(),
            Date = presc.Date,
            DueDate = presc.DueDate,
        };
    }

    private async Task<GetPatientDTO> AddNewPatientAsync(Patient prescriptionDtoCreatePatient)
    {
        //dla pewnosci sprawdzenie ze pacjent na pewno nie istnieje
        var patient = data.Patients.FirstOrDefault(p => p.IdPatient == prescriptionDtoCreatePatient.IdPatient);
        if (patient is null)
        {
            patient = prescriptionDtoCreatePatient;
            await data.Patients.AddAsync(patient);
            await data.SaveChangesAsync(); // zapisanie nowego pacjenta
        }
        return new GetPatientDTO
        {
            IdPatient = prescriptionDtoCreatePatient.IdPatient,
            FirstName = prescriptionDtoCreatePatient.FirstName,
            LastName = prescriptionDtoCreatePatient.LastName,
            BirthDate = prescriptionDtoCreatePatient.BirthDate,
        };
    }

    //todo getter dla pacjentow
}