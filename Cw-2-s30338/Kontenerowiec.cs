using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public class Kontenerowiec
    {
        private List<Kontener> kontenery;
        private double maksymalnaPredkosc;
        private int maksymalnaLiczbaKontenerow;
        private double maksymalneObciazenie;
        private int index;
        private static int autonumeracja=0;
        public Kontenerowiec(Kontener[] kontenery, double maksymalnaPredkosc, int maksymalnaLiczbaKontenerow, double maksymalneObciazenie)
        {
            this.kontenery = new List<Kontener>();
            this.maksymalnaPredkosc = maksymalnaPredkosc;
            this.maksymalnaLiczbaKontenerow = maksymalnaLiczbaKontenerow;
            this.maksymalneObciazenie = maksymalneObciazenie;
            this.index = autonumeracja;
            autonumeracja++;
        }
        public Kontenerowiec(double maksymalnaPredkosc, int maksymalnaLiczbaKontenerow, double maksymalneObciazenie)
        {
            this.kontenery = new List<Kontener>();
            this.maksymalnaPredkosc = maksymalnaPredkosc;
            this.maksymalnaLiczbaKontenerow = maksymalnaLiczbaKontenerow;
            this.maksymalneObciazenie = maksymalneObciazenie;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Kontenerowiec {index}: maksymalna predkosc: {maksymalnaPredkosc},\n max liczba kotenerow: {maksymalnaLiczbaKontenerow}," +
                $"maksymalne obciazenie: {maksymalneObciazenie}\n zawiera:\n");
            if (this.kontenery.Count == 0)
                sb.AppendLine("nie ma kontenerow na tym kontenerowcu");
            else 
                for (int i = 0; i < kontenery.Count; i++)
                {
                    var kontener = kontenery[i];
                    sb.AppendLine($"{kontener.NrSeryjny}, " +
                                  $"wysokość: {kontener.Wysokosc}, " +
                                  $"głębokość: {kontener.Glebokosc}, " +
                                  $"waga własna: {kontener.WagaWlasna}, " +
                                  $"maksymalna ładowność: {kontener.MaksymalnaLadownosc}");
                }
            return sb.ToString();
        }
        public string WypiszZawartoscKontenerowca()
        {
            StringBuilder sb = new StringBuilder(this.ToString());
            if (kontenery.Count == 0)
            {
                sb.AppendLine("na tym statku ne ma kontenerow");
            }
            for (int i = 0; i < kontenery.Count; i++)
            {
                sb.AppendLine(kontenery[i].ToString());
            }
            return sb.ToString();
        }
        public void StworzenieKontenera(string rodzajKontenera, 
                                        double wysokosc, 
                                        double glebokosc, 
                                        double wagaWlasna, 
                                        double maksymalnaLadownosc)
        {
            if (rodzajKontenera == "C")
                kontenery.Add(new KontenerChlodniczy_C(rodzajKontenera, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc));
            else if (rodzajKontenera == "G")
                kontenery.Add(new KontenerNaGaz_G(rodzajKontenera, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc));
            else if (rodzajKontenera == "L")
                kontenery.Add(new KontenerNaPlyny_L(rodzajKontenera, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc));
            else
                throw new NotKnownType("Podano nieznany typ kontenera");
        }
        public List<Kontener> Kontenery => kontenery;
        public void ZaladujNaStatek(Kontener kontener)
        {
            if (this.maksymalnaLiczbaKontenerow+1 <= this.maksymalnaLiczbaKontenerow)
            {
                kontenery.Add(kontener);
                Console.WriteLine($"Załadowano kontener {kontener.NrSeryjny} na kontenerowiec");
            }
            else
            {
                Console.WriteLine("kontenerowiec jest pełny.");
            }
        }
        public void ZaladujNaStatek(List<Kontener> kontener)
        {
            for(int i = 0; i < kontener.Count; i++)
            {
                ZaladujNaStatek(kontener[i]);
            }
        }
        public void ZdejmijZeStatku(Kontener kontener)
        {
            if(Kontenery.Contains(kontener))
            {
                Kontenery.Remove(kontener);
                Console.WriteLine("zdjeto kontener ze statku");
            }
            else
            {
                Console.WriteLine("nie ma takiego kontenera na statku");
            }
        }

    }
}
