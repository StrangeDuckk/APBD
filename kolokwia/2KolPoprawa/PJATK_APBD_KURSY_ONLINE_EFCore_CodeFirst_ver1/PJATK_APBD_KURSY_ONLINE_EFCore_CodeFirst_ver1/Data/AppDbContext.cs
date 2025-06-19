using Microsoft.EntityFrameworkCore;
using PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Models;

namespace PJATK_APBD_KURSY_ONLINE_EFCore_CodeFirst_ver1.Data;

public class AppDbContext:DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}