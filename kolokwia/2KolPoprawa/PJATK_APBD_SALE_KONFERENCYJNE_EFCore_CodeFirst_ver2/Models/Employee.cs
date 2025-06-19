using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;
[Table("Employee")]
[PrimaryKey(nameof(EmployeeId))]
public class Employee
{
    [Column("ID")]
    public int EmployeeId { get; set; }
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    [MaxLength(150)]
    public string Email { get; set; } = null!;
    
    // ------------ nawigacyjne ------------
    public virtual ICollection<RoomAccess> RoomAccesses { get; set; } = null!;
    public virtual ICollection<RoomBooking> RoomBookings { get; set; } = null!;
}