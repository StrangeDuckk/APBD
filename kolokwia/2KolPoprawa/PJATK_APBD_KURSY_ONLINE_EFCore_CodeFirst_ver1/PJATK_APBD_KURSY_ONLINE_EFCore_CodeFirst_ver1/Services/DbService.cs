using Microsoft.EntityFrameworkCore;
using PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Data;
using PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.DTOs;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Services;

public interface IDbService
{
    public Task<GetCourseWithEnrollmentsAndAssignedStudents?> GetCoursesByIdAsync(int id);
    public Task<string?> DeleteCourseByIdAsync(int id);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<GetCourseWithEnrollmentsAndAssignedStudents?> GetCoursesByIdAsync(int id)
    {
        // ---------- sprawdzenie istnienia kursu -----------
        var courses = await data.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
        if (courses == null)
            return null;
        
        // ---------- kurs istnieje, sciagniecie osob ktore go zarezerwowaly --------
        var enrollments = await data.Enrollments
            .Where(e => e.CourseId == id)
            .Select(e => new GetEnrollmentsDto()
            {
                Student = new GetStudentDto()
                {
                    Id = e.StudentId,
                    Name = e.Student.Name,
                    Email = e.Student.Email,
                },
                EnrollmentDate = e.EnrollmentDate,
                CompletionStatus = e.CompletionStatus,
            }).ToListAsync();
        
        // ------- sciagniecie osob ktore maja do niego dostep -------------
        var assignedStudents = await data.StudentCourses
            .Where(s => s.CourseId == id)
            .Select(s => new GetStudentDto()
            {
                Id = s.StudentId,
                Name = s.Student.Name,
                Email = s.Student.Email,
            }).ToListAsync();

        return new GetCourseWithEnrollmentsAndAssignedStudents()
        {
            Id = id,
            Title = courses.Title,
            Category = courses.Category,
            Enrollments = enrollments,
            AssignedStudents = assignedStudents
        };
    }

    public async Task<string?> DeleteCourseByIdAsync(int id)
    {
        // ------- usuwanie niebezpieczne -> wszsytko w transakcji --------
        var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            // ---------- sprawdzenie czy kurs istnieje ------------
            var course = await data.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null)
                return null;
            
            // ----------- usuniecie enrollmentow -----------
            var enrollmentsForCourse = await data.Enrollments.Where(e => e.CourseId == id).ToListAsync();
            data.Enrollments.RemoveRange(enrollmentsForCourse);
            
            // ----------- usuniecie studentCourse ----------
            var studentCourseToDelete = await data.StudentCourses.Where(s => s.CourseId == id).ToListAsync();
            data.StudentCourses.RemoveRange(studentCourseToDelete);
            // ----------- usuniecie kursu --------------
            data.Courses.Remove(course);
            
            await data.SaveChangesAsync();
            
            await transaction.CommitAsync();

            return $"Deleted Course with id: {id}";
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}