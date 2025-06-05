using Cw_10_s30338.Models;

namespace Cw_10_s30338.DTOs;

public class GetClientTripDTO
{
    public ClientDTO client { get; set; }
    public TripDTO trip { get; set; }
}