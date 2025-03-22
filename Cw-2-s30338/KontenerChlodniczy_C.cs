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
        public void ZaladujKontener(string RodzajProduktu, double masa)
        {
            if (masa > MaksymalnaLadownosc || masa < 0)
            {
                NiebezpiecznaSytuacja(NrSeryjny);
                throw new OverfillException($"Przekroczono dozwolony limit wypelnienia dla kontenera: {NrSeryjny}");
            }

        }
    }
}
