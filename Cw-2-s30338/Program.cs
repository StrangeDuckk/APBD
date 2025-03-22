using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Hello world");
        }
    }

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
        public abstract void ZaladujKontener(double masa);
        public abstract void OproznijKontener();
        public string NrSeryjny => nrSeryjny;
        public double MasaLadunku => masaLadunku;
        public void SetMasaLadunku(double masa)
        {
            this.masaLadunku = masa;
        }
    }
    public interface IHazardNotifier
    {
        void NiebezpiecznaSytuacja(string nrKontenera); //problem z interfacem: CS8701 i CS8370
    }
    public class KontenerNaPlyny_L : Kontener,IHazardNotifier
    {
        /*
        Kontenery na płyny pozwalają na przewożenie ładunku niebezpiecznego (np. paliwo) i ładunku zwykłego (np. mleko).
        Kontenery tego typu powinny implementować interfejs IHazardNotifier
        Interfejs ten pozwala na wysłanie notyfikacji tekstowej w trakcie zajścia niebezpiecznej sytuacji wraz z informacją o
        numerze kontenera.
        W momencie uruchomienia metody ładującej towary do kontenera powinniśmy:
        Jeśli kontener przechowuje niebezpieczny ładunek - możemy go wypełnić jedynie do 50% pojemności
        W innym wypadku możemy go wypełnić do 90% jego pojemności
        Jeśli naruszymy dowolną z opisanych reguł - powinniśmy zgłosić informacje o próbie wykonania niebezpiecznej
        operacji.
         */
        public bool czyZawartoscNiebezpieczna;
        public KontenerNaPlyny_L(string typKonteneru, double wysokosc, double glebokosc, double wagaWlasna, double maksymalnaLadownosc) 
            : base(typKonteneru, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc)
        {
            Console.WriteLine("zaimplementowano kontener na plyny (L) o nr: " + NrSeryjny);
        }

        public void NiebezpiecznaSytuacja(string nrKontenera)
        {
            throw new NotImplementedException();
            //todo zaimplemetnowac niebezpiecna sytuacje
        }

        public override void OproznijKontener()
        {
            SetMasaLadunku(0);
        }

        public override void ZaladujKontener(double masa)
        {
            //todo zaladowac kontener
            Console.WriteLine("Doszlo do niebezpiecznej sytuacji w kontenerze: " + NrSeryjny);
        }
    }

    class MyCustomException : Exception
    {
        public MyCustomException(string message) : base(message) {}
    }
}
