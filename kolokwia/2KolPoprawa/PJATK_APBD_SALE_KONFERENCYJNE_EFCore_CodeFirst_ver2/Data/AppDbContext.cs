using Microsoft.EntityFrameworkCore;
using PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Data;

public class AppDbContext:DbContext
{
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<RoomAccess> RoomAccesses { get; set; }
    public DbSet<RoomBooking> RoomBookings { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}