using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_Kolokwium_EFCore___CodeFirst_ver._3.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
}