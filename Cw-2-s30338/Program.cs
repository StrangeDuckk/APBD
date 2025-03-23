using System;
using System.Collections;
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
            var listaKontenerowcow = new List<Kontenerowiec>();
            var listaKontenerow = new List<Kontener>();
            Console.WriteLine("witaj w zarzadzaniu kontenerami");
            wypisanieOpcji(listaKontenerowcow,listaKontenerow);
            var input = int.Parse(Console.ReadLine());

            while(input != 11)
            {
                switch (input)
                {
                    case 0:     // Stworzenie Kontenerowca 
                        StworzenieKontenerowca(listaKontenerowcow);
                        break;
                    case 1:     // Stworzenie kontenera danego typu
                        StworzenieKontenera(listaKontenerow);
                    case 2:     // Załadowanie ładunku do danego kontenera 
                        ZaladowanieDoKontenera();
                    case 3:     // Załadowanie kontenera na statek 
                        ZaladowanieNaStatek();
                    case 4:     // Załadowanie listy kontenerów na statek 
                        ZaladowanieListyNaStatek();
                    case 5:     // Usunięcie kontenera ze statku 
                        UsuniecieKontenera();
                    case 6:     // Rozładowanie kontenera 
                        RozladowanieKontenera();
                    case 7:     // Zastąpienie kontenera na statku o danym numerze innym kontenerem 
                        ZastapienieKonteneraInnym();
                    case 8:     // Możliwość przeniesienie kontenera między dwoma statkami 
                        PrzeniesienieKonteneraMiedzyStatkami();
                    case 9:     // Wypisanie informacji o danym kontenerze 
                        WypisanieInformacjiOKontenerze();
                    case 10:    // Wypisanie informacji o danym statku i jego ładunku 
                        WypisanieInformacjiOStatkuILadunku();
                    default:
                        Console.WriteLine("podaj wartosc ponownie:");
                        break;
                }
                wypisanieOpcji(listaKontenerowcow, listaKontenerow);
            }
            Console.WriteLine("Dziekuje za wspolprace z baza :)");
        }

        

        public static void wypisanieOpcji(List<Kontenerowiec> listaKontenerowcow, List<Kontener> listaKontenerow)
        {
            Console.WriteLine("Lista kontenerowcow: ");
            if (listaKontenerowcow.Count == 0)
            {
                Console.WriteLine("Brak");
            }
            else
                for (int i = 0; i < listaKontenerowcow.Count; i++)
                    Console.WriteLine(listaKontenerowcow[i].ToString());

            Console.WriteLine("Lista kontenerow: ");
            if (listaKontenerow.Count == 0)
            {
                Console.WriteLine("Brak");
            }
            else
                for (int i = 0; i < listaKontenerowcow.Count; i++)
                    Console.WriteLine(listaKontenerowcow[i].ToString());

            Console.WriteLine("Mozliwe akcje:\n0. stworzenie kontenerowca,\n" +
                "1. stworzenie kontenera,\n2. zaladowanie ladunku do kontenera, \n3. zaladowanie konenera na statek, \n4. zaladowanie listy kontenerow na statek,\n" +
                "5. usuniecie kontenera ze statku, \n6. rozladowanie kontenera, \n7.zastapienie kontenera na statku o danym numerze innym kontenerem, \n" +
                "8. przeniesienie kontenera miedzy dwoma statkami, \n9. wypisanie informacji o danym kontenerze, \n10. wypisanie informacji o danym statku i jego ladunku,\n" +
                "11. zakonczenie pracy z baza");
        }
        private static void StworzenieKontenerowca(List<Kontenerowiec> listakontenerowcow)
        {
            Console.WriteLine("podaj dane do stworzenia kontenerowca:\nmaksymalna predkosc:");
            var maxPredkosc = double.Parse(Console.ReadLine());
            Console.WriteLine("maksymalna liczba kontenerow:");
            var maxLiczbaKont = int.Parse(Console.ReadLine());
            Console.WriteLine("maksymalne obciazenie:");
            var maxObciazenie = double.Parse(Console.ReadLine());

            listakontenerowcow.Add(new Kontenerowiec(maxPredkosc, maxLiczbaKont, maxObciazenie));
            Console.WriteLine("dodano kontenerowiec!");
        }
        private static void StworzenieKontenera(List<Kontener> listaKontenerow)
        {
            Console.WriteLine("podaj rodzaj kontenera (L,C,G): ");
            var rodzaj = Console.ReadLine();
            Console.WriteLine("podaj wysokosc");
            var wys = double.Parse(Console.ReadLine());
            Console.WriteLine("podaj glebokosc");
            var gl = double.Parse(Console.ReadLine());
            Console.WriteLine("podaj wage wlasna");
            var ww = double.Parse(Console.ReadLine());
            Console.WriteLine("podaj maksymalna ladownosc");
            var maxl = double.Parse(Console.ReadLine());

            if (rodzaj == "C")
                listaKontenerow.Add(new KontenerChlodniczy_C(rodzaj, wys, gl, ww, maxl));
            else if (rodzaj == "L")
                listaKontenerow.Add(new KontenerNaPlyny_L(rodzaj, wys, gl, ww, maxl));
            else if (rodzaj == "G")
                listaKontenerow.Add(new KontenerNaGaz_G(rodzaj, wys, gl, ww, maxl));
            else
                                Console.WriteLine("podano niepoprawny typ kotenera!!!");
        }
    }
}
