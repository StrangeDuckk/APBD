using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Models;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Data;

public class AppDbContext:DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}