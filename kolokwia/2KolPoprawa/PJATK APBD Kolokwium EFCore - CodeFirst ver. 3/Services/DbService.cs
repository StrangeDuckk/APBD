using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Data;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Services;

public interface IDbService
{
    Task<ICollection<GetCoursesDetailsDto>> GetAllCoutsesAsync();
    Task<StudentWithEnrollmentsGetDto> PostStudentWithEnrollmentsAsync(CreateStudentWithEnrollmentDto studentwithEnrollments);
}
public class DbService(AppDbContext data):IDbService
{
    public async Task<ICollection<GetCoursesDetailsDto>> GetAllCoutsesAsync()
    {
        return await data.Courses.Select(c => new GetCoursesDetailsDto
        {
            Id = c.CourseId,
            Title = c.Title,
            Teacher = c.Teacher,
            Student = data.Enrollments.Select(e => new StudentGetDto
            {
                Id = e.StudentId,
                FirstName = e.Student.FirstName,
                LastName = e.Student.LastName,
                Email = e.Student.Email,
                EnrollmentDate = e.EnrollmentDate
            }).ToList()
        }).ToListAsync();
    }

    public async Task<StudentWithEnrollmentsGetDto> PostStudentWithEnrollmentsAsync(CreateStudentWithEnrollmentDto body)
    {
        // zawsze tworzy nowego studenta -> transakcja
        await using var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            // ----------- utworzenie nowego studenta -----------
            var newStudent = new Student
            {
                FirstName = body.FirstName,
                LastName = body.LastName,
                Email = body.Email
            };
            // ----------- dodanie nowego studenta do bazy ---------
            await data.Students.AddAsync(newStudent);
            await data.SaveChangesAsync();
            
            // ----------- zebranie listy kursow z body ---------
            var courses = new List<GetCoursesDetailsDto>();
            foreach (var courseFromBody in body.Courses)
            {
                // ----------- sprawdzenie istnienia kursu --------------
                var course = await data.Courses.FirstOrDefaultAsync(c => 
                    c.Title == courseFromBody.Title &&
                    c.Teacher == courseFromBody.Teacher &&
                    c.Credits == courseFromBody.Credits);
                if (course == null)
                {
                    // ----------- dodanie kursu do bazy -----------
                    var c = new Course
                    {
                        Title = course.Title,
                        Teacher = course.Teacher,
                        Credits = course.Credits
                    };
                    await data.Courses.AddAsync(c);
                    await data.SaveChangesAsync();
                }
                
                // ---------- dodanie enrollmentu do bazy ----------
                var enrollment = new Enrollment
                {
                    StudentId = newStudent.StudentId,
                    CourseId = course.CourseId,
                    EnrollmentDate = DateTime.Now,
                };
                await data.Enrollments.AddAsync(enrollment);
                await data.SaveChangesAsync();
                
                // ---------- zebranie danych do wypisania --------
                courses.Add(new GetCoursesDetailsDto
                {
                    Id = course.CourseId,
                    Title = course.Title,
                    Teacher = course.Teacher,
                    Credits = course.Credits
                });
            }
            await transaction.CommitAsync();

            return new StudentWithEnrollmentsGetDto
            {
                Student = new StudentGetDto
                {
                    Id = newStudent.StudentId,
                    FirstName = newStudent.FirstName,
                    LastName = newStudent.LastName,
                    Email = newStudent.Email
                },
                Courses = courses
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}