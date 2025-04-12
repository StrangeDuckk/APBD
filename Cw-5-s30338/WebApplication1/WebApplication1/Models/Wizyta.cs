namespace WebApplication1.Models
{
    public class Wizyta
    {
        public int Id { get; set; }
        public DateTime dataWizyty {  get; set; }
        public string opisWizyty { get; set; }
        public double cenaWizyty { get; set; }
        public Zwierze zwierzeNaWizycie {  get; set; }
    }
}
