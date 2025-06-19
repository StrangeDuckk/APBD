using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;

[Table("RoomBooking")]
[PrimaryKey(nameof(RoomId),nameof(EmployeeId))]
public class RoomBooking
{
    [Column("Room_ID")]
    public int RoomId { get; set; }
    
    [Column("Employee_ID")]
    public int EmployeeId { get; set; }
    
    public DateTime BookingStart { get; set; }
    public DateTime? BookingEnd { get; set; }
    
    // ----------- nawigacyjne -------------
    [ForeignKey(nameof(RoomId))]
    public virtual Room Room { get; set; } = null!;
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; } = null!;
}