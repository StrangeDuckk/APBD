namespace Cw_7_s30338.Models.DTOs;

public class TripCountryGetDTO
{
    public int IdTrip { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public int IdCountry { get; set; }
    public string Country { get; set; }
}