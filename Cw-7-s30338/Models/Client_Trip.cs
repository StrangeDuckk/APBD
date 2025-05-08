namespace Cw_7_s30338.Models;

public class Client_Trip
{
    //sledzi ktorzy klienci sa zarejestrowani na ktore wycieczki
    //potrzeba get i create
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public int RegisteredAt { get; set; }
    public int PaymentDate { get; set; }
}