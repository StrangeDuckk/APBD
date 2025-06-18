using kolokwium2Przykladowe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace kolokwium2Przykladowe.Data;

public class AppDbContext:DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        /*
        var student = new Student
        {
            Id = 1,
            FirstName = "jan",
            LastName = "nowak",
            Email = "jan.nowak@gmail.com"
        };
        
        modelBuilder.Entity<Student>().HasData(student);
        */
    }
}