using System.Runtime.CompilerServices;

namespace Cw_7_s30338.Models.DTOs;

public class Client_TripCreateDTO
{
    public required int IdClient { get; set; }
    public required int IdTrip { get; set; }
    public required int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; } //paymentDate jest nullable
}