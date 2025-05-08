namespace Cw_7_s30338.Models;

public class Client_Trip
{
    //sledzi ktorzy klienci sa zarejestrowani na ktore wycieczki
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime PaymentDate { get; set; }
}