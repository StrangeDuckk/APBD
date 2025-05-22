using Cw_9_s30338.Models;
using Microsoft.EntityFrameworkCore;

namespace Cw_9_s30338.Data;

public class AppDbContext :DbContext
{
    public DbSet<Medicament> Medicaments { get; set; }//operacje na tym jak na rzeczywistej tabeli
    
    public AppDbContext(DbContextOptions options):base(options)
    {
    }
}