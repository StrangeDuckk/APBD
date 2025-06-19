using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Data;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.DTOs;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Services;

public interface IDbService
{
    Task<ICollection<GetCoursesDetailsDto>> GetAllCoutsesAsync();
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
}