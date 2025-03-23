using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public class KontenerNaGaz_G : Kontener, IHazardNotifier
    {
        private double cisnienie;
        public KontenerNaGaz_G(string typKonteneru, double wysokosc, double glebokosc, double wagaWlasna, double maksymalnaLadownosc) 
            : base(typKonteneru, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc)
        {
            Console.WriteLine("utworzono kontener na gaz: " + NrSeryjny);
        }
        public void NiebezpiecznaSytuacja(string nrKontenera)
        {
            Console.WriteLine("Doszlo do niebezpiecznej sytuacji w kontenerze: " + NrSeryjny);
        }
        public override void OproznijKontener()
        {
            SetMasaLadunku(0.05 * MasaLadunku); //pozostawiamy 5% calego ladunku wewnatrz kontenera
            SetCzyZaladowany(false);
            Console.WriteLine($"oprozniono kontener {NrSeryjny} z pozostawieniem 5% ladunku wewnatrz");
        }

        public override string ToString()
        {
            string tekst = $"Kontener {NrSeryjny}, wysokosc {Wysokosc}, waga {WagaWlasna}, glebokosc {Glebokosc}, maksymalna ladownosc {MaksymalnaLadownosc},\n";
            if (!CzyZaladowany)
                tekst += "kontener jest pusty";
            else
                tekst += $"kontener zawiera: produkt, o masie: {MasaLadunku}";
            return tekst;
        }

        public override string TypKontenera()
        {
            return "NaGaz_G";
        }
        public void ZaladujKontener(double masa, double cisnienie)
        {
            if (masa > MaksymalnaLadownosc || masa < 0)
            {
                NiebezpiecznaSytuacja(NrSeryjny);
                throw new OverfillException($"przekroczono ladownosc kontenera {NrSeryjny}");
            }
            if (CzyZaladowany)
            {
                throw new AlreadyFilledKontener("ten kontener juz jest wypelniony gazem!");
            }
            else
            {
                SetMasaLadunku(masa);
                SetCzyZaladowany(true);
                this.cisnienie = cisnienie;
                Console.WriteLine($"zaladowano kontener {NrSeryjny}");
            }
        }
    }
}
