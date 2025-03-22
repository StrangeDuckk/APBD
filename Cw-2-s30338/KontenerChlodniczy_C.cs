using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public class KontenerChlodniczy_C : Kontener, IHazardNotifier
    {
        private string RodzajProduktu;
        private double Temperatura;
        private readonly Dictionary<string, double> ProduktyITemperatury;
        public KontenerChlodniczy_C(string typKonteneru, double wysokosc, double glebokosc, double wagaWlasna, double maksymalnaLadownosc) 
            : base(typKonteneru, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc)
        {
            Console.WriteLine("utworzono kontener chlodniczy: " + NrSeryjny);

            ProduktyITemperatury = new Dictionary<string, double>
            {
                {"Bananas", 13.3},
                {"Chocolate", 18.0},
                {"Fish", 2.0},
                {"Meat", -15.0},
                {"Ice cream", -18.0},
                {"Frozen pizza", -30},
                {"Cheese", 7.2},
                {"Sausages", 5.0},
                {"Butter", 20.5},
                {"Eggs", 19.0}
            };
        }
        public void NiebezpiecznaSytuacja(string nrKontenera)
        {
            Console.WriteLine("Doszlo do niebezpiecznej sytuacji w kontenerze: " + NrSeryjny);
        }
        public override void OproznijKontener()
        {
            SetMasaLadunku(0);
        }
        public void ZaladujKontener(string RodzajProduktu,double Temperatura, double masa)
        {
            if(!ProduktyITemperatury.ContainsKey(RodzajProduktu))
            {
                throw new NotKnownType($"{NrSeryjny}, nie mozna dodawac produktu: {RodzajProduktu}, nieznany typ");
            }
            if (masa > MaksymalnaLadownosc || masa < 0)
            {
                NiebezpiecznaSytuacja(NrSeryjny);
                throw new TooHighTemperature($"Przekroczono dozwolony limit wypelnienia dla kontenera: {NrSeryjny}");
            }
            if(Temperatura> ProduktyITemperatury[RodzajProduktu])
            {
                NiebezpiecznaSytuacja(NrSeryjny);
                throw new TooHighTemperature($"{NrSeryjny}, Temperatura {Temperatura} jest za wysoka dla produktu {RodzajProduktu}!\n" +
                    $"Maksymalna temperatura: {ProduktyITemperatury[RodzajProduktu]}");
            }

            this.RodzajProduktu = RodzajProduktu;
            this.Temperatura = Temperatura;
            SetMasaLadunku(masa);
            Console.WriteLine($"zaladowano kontener {NrSeryjny} produktem {RodzajProduktu}");
        }
    }
}
