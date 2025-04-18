﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw_2_s30338
{
    public class KontenerNaPlyny_L : Kontener, IHazardNotifier
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
        private bool czyZawartoscNiebezpieczna;
        public KontenerNaPlyny_L(string typKonteneru, double wysokosc, double glebokosc, double wagaWlasna, double maksymalnaLadownosc)
            : base(typKonteneru, wysokosc, glebokosc, wagaWlasna, maksymalnaLadownosc)
        {
            Console.WriteLine("utworzono kontener na plyny: " + NrSeryjny);
        }

        public void NiebezpiecznaSytuacja()
        {
            Console.WriteLine("Doszlo do niebezpiecznej sytuacji w kontenerze: " + NrSeryjny);
        }
        public override void OproznijKontener()
        {
            SetMasaLadunku(0);
            SetCzyZaladowany(false);
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
            return "NaPlyny_L";
        }
        public void ZaladujKontener(double masa, bool czyNiebezpieczny)
        {
            this.czyZawartoscNiebezpieczna = czyNiebezpieczny;
            if (CzyZaladowany)
                Console.WriteLine($"UWAGA!!!: ten kontener juz zawiera plyn!");
            if (this.czyZawartoscNiebezpieczna)
            {
                Console.WriteLine("ZAWARTOSC NIEBEZPIECZNA, kontener mozna wypelnic tylko w 50%, czyli " + MaksymalnaLadownosc * 0.5);
                if (masa > MaksymalnaLadownosc * 0.5 || masa < 0)
                {
                    NiebezpiecznaSytuacja();
                    Console.WriteLine($"UWAGA!!!: proba wypelnienia kontenera " + NrSeryjny + " niebezpieczna substancja powyzej 50% pojemniosci!");
                }
                else
                {
                    SetMasaLadunku(masa);
                    SetCzyZaladowany(true);
                    Console.WriteLine("zaladowano kontener: " + NrSeryjny);
                }
            }
            else
            {
                Console.WriteLine("zawartosc zwykla, kontener mozna wypelnic w 90%, czyli: " + MaksymalnaLadownosc * 0.9);
                if (masa > MaksymalnaLadownosc * 0.9 || masa < 0)
                {
                    NiebezpiecznaSytuacja();
                    Console.WriteLine($"UWAGA!!!: proba wypelnienia kontenera " + NrSeryjny + "zwykla substancja powyzej 90% pojemnosci!");
                }
                else
                {
                    SetMasaLadunku(masa);
                    SetCzyZaladowany(true);
                    Console.WriteLine("zaladowano kontener: " + NrSeryjny);
                }
            }
        }
    }
}
