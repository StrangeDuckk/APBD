using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PJATK_APBD_SALE_KONFERENCYJNE_EFCore_CodeFirst_ver2.Models;

[Table("Room")]
[PrimaryKey(nameof(RoomId))]
public class Room
{
    [Column("ID")]
    public int RoomId { get; set; }
    [MaxLength(10)]
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    //----------------- nawigacyjne -----------
    public virtual ICollection<RoomAccess> RoomAccesses { get; set; } = null!;
    public virtual ICollection<RoomBooking> RoomBookings { get; set; } = null!;
}