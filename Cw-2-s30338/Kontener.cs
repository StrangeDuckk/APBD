using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public abstract class Kontener
    {
        /*
         Masę ładunku (w kilogramach)
         Wysokość (w centymetrach)
         Waga własna (waga samego kontenera, w kilogramach)
         Głębokość (w centymetrach)
         Numer seryjny
         Format numeru to KON-C-1
         Pierwszy człon numery to zawsze "KON"
         Drugi człon reprezentuje rodzaj kontenera
         Trzeci człon to liczba. Liczby powinny być unikalne. Nie powinno być możliwości powstania dwóch kontenerów o tym
         samym numerze. Numery powinny być generowane przez system.
         Maksymalna ładowność danego kontenera w kilogramach
         */
        private double masaLadunku;
        private double wysokosc;
        private double wagaWlasna;
        private double glebokosc;
        private string nrSeryjny; // format: "KON-RODZAJ-AUTONUMERACJA"
        private double maksymalnaLadownosc;
        private static int autonumeracja = 0;

        public Kontener(string typKonteneru, double wysokosc, double glebokosc, double wagaWlasna, double maksymalnaLadownosc)
        {
            this.wysokosc = wysokosc;
            this.wagaWlasna = wagaWlasna;
            this.glebokosc = glebokosc;
            this.maksymalnaLadownosc = maksymalnaLadownosc;
            this.nrSeryjny = "KON-" + typKonteneru + autonumeracja;
            autonumeracja++;
        }
        public abstract void ZaladujKontener(double masa, bool czyNiebezpieczny);
        public abstract void OproznijKontener();
        public string NrSeryjny => nrSeryjny;
        public double MaksymalnaLadownosc => maksymalnaLadownosc;
        public double MasaLadunku => masaLadunku;
        public void SetMasaLadunku(double masa)
        {
            this.masaLadunku = masa;
        }
    }
}
