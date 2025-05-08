namespace Cw_7_s30338.Models;

public class Trip
{
    //szczegoly pakietow podrozy
    //potrzeba tylko getDTO
    public int IdTrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
}