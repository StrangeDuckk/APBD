using System;
using System.Collections.Generic;
using System.Linq;

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
                        break;
                    case 2:     // Załadowanie ładunku do danego kontenera 
                        ZaladowanieDoKontenera(listaKontenerow, listaKontenerowcow);
                        break;
                    case 3:     // Załadowanie kontenera na statek 
                        ZaladowanieNaStatek(listaKontenerowcow, listaKontenerow);
                        break;
                    case 4:     // Załadowanie listy kontenerów na statek 
                        ZaladowanieListyNaStatek(listaKontenerowcow, listaKontenerow);
                        break;
                    case 5:     // Usunięcie kontenera ze statku 
                        UsuniecieKontenera(listaKontenerowcow);
                        break;
                    case 6:     // Rozładowanie kontenera 
                        RozladowanieKontenera(listaKontenerow, listaKontenerowcow);
                        break;
                    case 7:     // Zastąpienie kontenera na statku o danym numerze innym kontenerem 
                        ZastapienieKonteneraInnym(listaKontenerowcow);
                        break;
                    case 8:     // Możliwość przeniesienie kontenera między dwoma statkami 
                        PrzeniesienieKonteneraMiedzyStatkami(listaKontenerowcow);
                        break;
                    case 9:     // Wypisanie informacji o danym kontenerze 
                        WypisanieInformacjiOKontenerze(listaKontenerowcow,listaKontenerow);
                        break;
                    case 10:    // Wypisanie informacji o danym statku i jego ładunku 
                        WypisanieInformacjiOStatkuILadunku(listaKontenerowcow);
                        break;
                    default:
                        Console.WriteLine("podaj wartosc ponownie:");
                        break;
                }
                wypisanieOpcji(listaKontenerowcow, listaKontenerow);
                input = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Dziekuje za wspolprace z baza :)");
        }
        public static void wypisanieOpcji(List<Kontenerowiec> listaKontenerowcow, List<Kontener> listaKontenerow)
        {
            Console.WriteLine("------------------------------------------------------------");
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
                for (int i = 0; i < listaKontenerow.Count; i++)
                    Console.WriteLine(listaKontenerow[i].ToString());

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
        private static void ZaladowanieDoKontenera(List<Kontener> listaKontenerow, List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj numer seryjny kontenera do ktorego chcesz dodac produkty:");
            var nrSeryjny = Console.ReadLine();

            var kontener = listaKontenerow.FirstOrDefault(kon => kon.NrSeryjny == nrSeryjny);
            if (kontener != null)
            {
                Console.WriteLine("znalenziono kontener, jest w porcie");

                if (kontener is KontenerNaGaz_G konG)
                {
                    Console.WriteLine("podaj mase gazu:");
                    var masa = double.Parse(Console.ReadLine());
                    Console.WriteLine("podaj cisnienie:");
                    var cisnienie = double.Parse(Console.ReadLine());

                    konG.ZaladujKontener(masa, cisnienie);
                }
                else if (kontener is KontenerChlodniczy_C konC)
                {
                    Console.WriteLine("podaj rodzaj produktu:");
                    var rodzaj = Console.ReadLine();
                    Console.WriteLine("podaj temperature produktu:");
                    var masa = double.Parse(Console.ReadLine());
                    Console.WriteLine("podaj cisnienie:");
                    var cisnienie = double.Parse(Console.ReadLine());

                    konC.ZaladujKontener(rodzaj, masa, cisnienie);
                }
                else if (kontener is KontenerNaPlyny_L konL)
                {
                    Console.WriteLine("podaj mase plynu:");
                    var masa = double.Parse(Console.ReadLine());
                    Console.WriteLine("podaj czy niebezpieczny (true/false):");
                    var niebezpieczny = bool.Parse(Console.ReadLine());

                    konL.ZaladujKontener(masa, niebezpieczny);
                }
            }
            else if (kontener == null)
                Console.WriteLine("kontener o tyn numerze nie istnieje");
            else
            {
                for (int i = 0; i < listaKontenerowcow.Count; i++)
                {
                    var kontenerZeStatku = listaKontenerowcow[i].Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrSeryjny);
                    if (kontenerZeStatku != null)
                    {
                        Console.WriteLine("znalenziono kontener, jest na statku");

                        if (kontener is KontenerNaGaz_G konG)
                        {
                            Console.WriteLine("podaj mase gazu:");
                            var masa = double.Parse(Console.ReadLine());
                            Console.WriteLine("podaj cisnienie:");
                            var cisnienie = double.Parse(Console.ReadLine());

                            konG.ZaladujKontener(masa, cisnienie);
                        }
                        else if (kontener is KontenerChlodniczy_C konC)
                        {
                            Console.WriteLine("podaj rodzaj produktu:");
                            var rodzaj = Console.ReadLine();
                            Console.WriteLine("podaj temperature produktu:");
                            var masa = double.Parse(Console.ReadLine());
                            Console.WriteLine("podaj cisnienie:");
                            var cisnienie = double.Parse(Console.ReadLine());

                            konC.ZaladujKontener(rodzaj, masa, cisnienie);
                        }
                        else if (kontener is KontenerNaPlyny_L konL)
                        {
                            Console.WriteLine("podaj mase plynu:");
                            var masa = double.Parse(Console.ReadLine());
                            Console.WriteLine("podaj czy niebezpieczny (true/false):");
                            var niebezpieczny = bool.Parse(Console.ReadLine());

                            konL.ZaladujKontener(masa, niebezpieczny);
                        }

                        break;
                    }
                }
            }
        }
        private static void ZaladowanieNaStatek(List<Kontenerowiec> listaKontenerowcow, List<Kontener> listaKontenerow)
        {
            Console.WriteLine("podaj statek na ktory chcesz zaladowac kontener");
            var nrStatku = int.Parse(Console.ReadLine());
            var statek = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatku);

            if (statek == null)
                Console.WriteLine("podany numer statku jest niepoprawny!");
            else
            {
                Console.WriteLine("podaj nrSeryjny kontenera, ktory chcesz zaladowac na statek");
                var nrSer = Console.ReadLine();
                var kontener = listaKontenerow.FirstOrDefault(kon => kon.NrSeryjny == nrSer);
                if (kontener == null)
                {
                    Console.WriteLine("podany numer kontenera jest niepoprawny!");
                }
                else
                {
                    statek.ZaladujNaStatek(kontener);
                }
            }
        }
        private static void ZaladowanieListyNaStatek(List<Kontenerowiec> listaKontenerowcow, List<Kontener> listaKontenerow)
        {
            Console.WriteLine("obecne kontenery na liscie: ");
            for (int i = 0; i < listaKontenerow.Count; i++)
            {
                listaKontenerow[i].ToString();
            }
            Console.WriteLine("podaj nr statku na ktory chcesz zaladowac ta liste kontenerow: ");
            var nrStatku = int.Parse(Console.ReadLine());
            var statek = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatku);

            if (statek == null)
                Console.WriteLine("podany numer statku jest niepoprawny!");
            else
                statek.ZaladujNaStatek(listaKontenerow);
        }
        private static void UsuniecieKontenera(List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj nr statku z ktorego chcesz usunac kontener: ");
            var nrStatku = int.Parse(Console.ReadLine());
            var statek = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatku);

            if (statek == null)
                Console.WriteLine("podany numer statku jest niepoprawny!");
            else
            {
                Console.WriteLine("podaj nrSeryjny kontenera, ktory chcesz usunac ze statku");
                var nrSer = Console.ReadLine();
                var kontener = statek.Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrSer);
                if (kontener == null)
                {
                    Console.WriteLine("podany numer kontenera jest niepoprawny!");
                }
                else
                {
                    statek.ZdejmijZeStatku(kontener);
                }
            }
        }
        private static void RozladowanieKontenera(List<Kontener> listaKontenerow, List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj numer seryjny kontenera z ktorego chcesz usunac produkty:");
            var nrSeryjny = Console.ReadLine();

            var kontener = listaKontenerow.FirstOrDefault(kon => kon.NrSeryjny == nrSeryjny);
            if (kontener != null)
            {
                Console.WriteLine("znalenziono kontener, jest w porcie");
                kontener.OproznijKontener();
            }
            else
            {
                for (int i = 0; i < listaKontenerowcow.Count; i++)
                {
                    var kontenerZeStatku = listaKontenerowcow[i].Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrSeryjny);
                    if (kontenerZeStatku != null)
                    {
                        Console.WriteLine("znalenziono kontener, jest na statku");

                        kontener.OproznijKontener();

                        break;
                    }
                }
            }
        }
        private static void ZastapienieKonteneraInnym(List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj nr statku na ktory chcesz wstawic kontener: ");
            var nrStatekNowy = int.Parse(Console.ReadLine());
            var statekNowy = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatekNowy);
            if (statekNowy == null)
                Console.WriteLine("podano niewlasciwy numer statku");
            else if (statekNowy.Kontenery.Count == statekNowy.MaksymalnaLiczbaKontenerow)
                Console.WriteLine("ten statek jest juz pelny");
            else
            {
                Console.WriteLine("podaj nr statku z ktorego chcesz zdjac kontener: ");
                var nrStatekStary = int.Parse(Console.ReadLine());
                var statekStary = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatekStary);
                if (statekStary == null)
                    Console.WriteLine("podano niewlasciwy numer statku");
                else if (statekStary.Kontenery.Count == statekStary.MaksymalnaLiczbaKontenerow)
                    Console.WriteLine("ten statek jest juz pelny");
                else
                {
                    Console.WriteLine("podaj numer seryjny kontenera ktory chcesz zdjac ze starego statku: ");
                    var nrKon = Console.ReadLine();
                    var kontenerstary = statekStary.Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrKon);
                    if (kontenerstary == null)
                        Console.WriteLine("kontener nie znajduje sie na tym statku!");
                    else
                    {
                        Console.WriteLine("podaj numer seryjny kontenera ktory chcesz zdjac z nowego statku: ");
                        var nrKonNowy = Console.ReadLine();
                        var kontenerNowy = statekNowy.Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrKon);
                        if (kontenerNowy == null)
                            Console.WriteLine("kontener nie znajduje sie na tym statku!");
                        else
                        {
                            statekStary.ZdejmijZeStatku(kontenerstary);
                            statekNowy.ZdejmijZeStatku(kontenerNowy);
                            statekStary.ZaladujNaStatek(kontenerNowy);
                            statekNowy.ZaladujNaStatek(kontenerstary);
                            Console.WriteLine("zamiana kontenerow powiodla sie");
                        }
                    }
                }
            }
        }
        private static void PrzeniesienieKonteneraMiedzyStatkami(List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj nr statku na ktory chcesz wstawic kontener: ");
            var nrStatekNowy = int.Parse(Console.ReadLine());
            var statekNowy = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatekNowy);
            if (statekNowy == null)
                Console.WriteLine("podano niewlasciwy numer statku");
            else if (statekNowy.Kontenery.Count == statekNowy.MaksymalnaLiczbaKontenerow)
                Console.WriteLine("ten statek jest juz pelny");
            else
            {
                Console.WriteLine("podaj nr statku z ktorego chcesz zdjac kontener: ");
                var nrStatekStary = int.Parse(Console.ReadLine());
                var statekStary = listaKontenerowcow.FirstOrDefault(stat => stat.Index == nrStatekStary);
                if (statekStary == null)
                    Console.WriteLine("podano niewlasciwy numer statku");
                else
                {
                    Console.WriteLine("podaj numer seryjny konputera ktory chcesz zdjac: ");
                    var nrKon = Console.ReadLine();
                    var kontener = statekStary.Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrKon);
                    if (kontener == null)
                        Console.WriteLine("kontener nie znajduje sie na tym statku!");
                    else
                    {
                        statekStary.ZdejmijZeStatku(kontener);
                        statekNowy.ZaladujNaStatek(kontener);
                        Console.WriteLine("przeniesienie kontenera miedzy statkami powiodlo sie");
                    }
                }
            }
        }
        private static void WypisanieInformacjiOKontenerze(List<Kontenerowiec> listaKontenerowcow, List<Kontener> listaKontenerow)
        {
            Console.WriteLine("podaj numer seryjny kontenera ktory chcesz wypisac: ");
            var nrKon = Console.ReadLine();
            var kontener = listaKontenerow.FirstOrDefault(kon => kon.NrSeryjny == nrKon);
            if (kontener == null)
            {
                for (int i = 0; i < listaKontenerowcow.Count; i++)
                {
                    var kontenerNaStatku = listaKontenerowcow[i].Kontenery.FirstOrDefault(kon => kon.NrSeryjny == nrKon);
                    if (kontenerNaStatku != null)
                    {
                        Console.WriteLine(kontenerNaStatku.ToString());
                    }
                }
            }
            else
            {
                kontener.ToString();
            }
        }
        private static void WypisanieInformacjiOStatkuILadunku(List<Kontenerowiec> listaKontenerowcow)
        {
            Console.WriteLine("podaj kontenerowiec o ktorym chcesz wiedziec WSZYSTKOO: ");
            var index = int.Parse(Console.ReadLine());
            var kontenerowiec = listaKontenerowcow.FirstOrDefault(kon => kon.Index == index);
            if (kontenerowiec == null)
                Console.WriteLine("nie istnieje kontenerowiec o takim numerze");
            else
            {
                Console.WriteLine(kontenerowiec.ToString());
                for (int i = 0; i < kontenerowiec.Kontenery.Count; i++)
                {
                    Console.WriteLine(kontenerowiec.Kontenery[i].ToString());
                }
            }
        }
    }
}
