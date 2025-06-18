using System.Collections;
using System.Data;
using kolokwium2Przykladowe.Data;
using kolokwium2Przykladowe.DTOs;
using kolokwium2Przykladowe.Exceptions;
using kolokwium2Przykladowe.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium2Przykladowe.Services;

public interface IDbService
{
    Task<IEnumerable<EnrollmentGetDTO>> EnrollmentGetAllAsync();
    Task<CourseWithEnrollmentsGetDTO> AddNewCourseWithStudentsAsync(CourseCreateDTO courseCreateDto);
}

public class DbService(AppDbContext data): IDbService
{
    public async Task<IEnumerable<EnrollmentGetDTO>> EnrollmentGetAllAsync()
    {
        //zebranie danych z potrzebnych tabel z bazy i walidacja
        var enrollments = data.Enrollments.AsEnumerable();
        if (!enrollments.Any())
        {
            throw new NotFound("There are no enrollments in the database");
        }
        var students = data.Students.AsEnumerable();
        if (!students.Any())
        {
            throw new NotFound("There are no students in the database");
        }
        var courses = data.Courses.AsEnumerable();
        if (!courses.Any())
        {
            throw new NotFound("There are no courses in the database");
        }
        
        // ---------------- stworzenie wynikowego enrollmentu -------------
        return await data.Enrollments
            .Include(s => s.Student)//
            .Include(c => c.Course)
            .Select(e => new EnrollmentGetDTO
            {
                StudentGetDTO = new StudentGetDTO
                {
                    Id = e.Student.Id,
                    FirstName = e.Student.FirstName,
                    LastName = e.Student.LastName,
                    Email = e.Student.Email,
                },
                CourseGetDTO = new CourseGetDTO{
                    Id = e.Course.Id,
                    Credits  = e.Course.Credits,
                    Teacher = e.Course.Teacher,
                    Title = e.Course.Title,
                },
                EnrollmentDate = e.EnrollmentDate,
            }).ToListAsync();
        
        /*List<EnrollmentGetDTO> listOfEnrollments = new List<EnrollmentGetDTO>();
        foreach (var enrollment in enrollments)
        {
            var courseForEnrollment = courses.FirstOrDefault(c => c.Id == enrollment.Course_Id);
            var studentForEnrollment = students.FirstOrDefault(s => s.Id == enrollment.Student_Id);
            listOfEnrollments.Add(new EnrollmentGetDTO
            {
                CourseGetDTO = new CourseGetDTO
                {
                    Id = courseForEnrollment.Id,
                    Title = courseForEnrollment.Title,
                    Credits = courseForEnrollment.Credits,
                    Teacher = courseForEnrollment.Teacher,
                },
                StudentGetDTO = new StudentGetDTO
                {
                    Id = studentForEnrollment.Id,
                    FirstName = studentForEnrollment.FirstName,
                    LastName = studentForEnrollment.LastName,
                    Email = studentForEnrollment.Email,
                },
                EnrollmentDate = enrollment.EnrollmentDate,
            });
        }

        return listOfEnrollments;*/
    }

    public async Task<CourseWithEnrollmentsGetDTO> AddNewCourseWithStudentsAsync(CourseCreateDTO courseCreateDto)
    {
        // ----------- sprawdzenie czy nie przeslano pustego body ---------
        if (courseCreateDto == null)
        {
            throw new NoNullAllowedException("Cannot send empty prescription");
        }
        
        // ------------- utworzenie nowego kursu i dodanie do bazy -----------------
        var course = new Course
        {
            Title = courseCreateDto.Course.Title,
            Credits = courseCreateDto.Course.Credits,
            Teacher = courseCreateDto.Course.Teacher,
        };
        
        await data.Courses.AddAsync(course);
        await data.SaveChangesAsync();
        string messagePart = "Kurs zostal utworzony";
        
        // ----------- dla kazdego studenta ------------
        // ----------- sprawdzenie istnienia studenta --------------
        var listOfEnrollments = new List<EnrollmentOnlyStudentGetDTO>(); // do wypisania odpowiedzi
        var enrollmentsToAdd = new List<Enrollment>();
        foreach (var studentDTO in courseCreateDto.Students)
        {
            // dla kazdego studenta sprawdzenie czy istnieje
            if (await data.Students.FirstOrDefaultAsync(s => s.Id == studentDTO.Id) == null)
            {
                // stworzenie nowego studenta
                await AddNewStudentAsync(new StudentCreateDTO()
                {
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName,
                    Email = studentDTO.Email,
                });
            }

            // student istnieje albo zostal stworzony
            // dodanie enrollmentu dla kazdego studenta
            
            enrollmentsToAdd.Add(new Enrollment
            {
                Course = course,
                Student = await data.Students.FirstOrDefaultAsync(s => s.Id == studentDTO.Id),//to juz nie bedzie nigdy null bo gdy null tworzony jest student wyzej
                Course_Id = course.Id,
                Student_Id = studentDTO.Id,
                EnrollmentDate = DateTime.Now
            });
            
            // dodanie enrollmentu do listy do wypisania
            listOfEnrollments.Add(new EnrollmentOnlyStudentGetDTO
            {
                EnrollmentDate = DateTime.Now,
                StudentGetDTO = new StudentGetDTO
                {
                    Id = studentDTO.Id,
                    FirstName = studentDTO.FirstName,
                    LastName = studentDTO.LastName,
                    Email = studentDTO.Email,
                },
            });
        }

        await data.Enrollments.AddRangeAsync(enrollmentsToAdd);
        await data.SaveChangesAsync();
        messagePart += " i studenci zostali zapisani";

        return new CourseWithEnrollmentsGetDTO()
        {
            message = messagePart,
            Course = new CourseGetDTO
            {
                Id = course.Id,
                Credits = course.Credits,
                Teacher = course.Teacher,
                Title = course.Title,
            },
            Enrollments = listOfEnrollments,
        };
    }

    private async Task<StudentGetDTO> AddNewStudentAsync(StudentCreateDTO student)
    {
        var students = data.Students.AsEnumerable();
        if (students.FirstOrDefault(s =>
                s.FirstName == student.FirstName &&
                s.LastName == student.LastName &&
                s.Email == student.Email) == null) // nie mamy dostepu do id przy create
        {
            //------------- dodanie nowego studenta ---------------
            await data.Students.AddAsync(new Student
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
            });
            await data.SaveChangesAsync();
        }
        var id = students.FirstOrDefault(s =>
            s.FirstName == student.FirstName &&
            s.LastName == student.LastName &&
            s.Email == student.Email).Id; // znalezienie jego id po dodaniu i zapisaiu zmian
        
        return new StudentGetDTO()
        {
            Id = students.FirstOrDefault(s => s.Id == id).Id,
            FirstName = students.FirstOrDefault(s => s.Id == id).FirstName,
            LastName = students.FirstOrDefault(s => s.Id == id).LastName,
            Email = students.FirstOrDefault(s => s.Id == id).Email,
        };
    }
}