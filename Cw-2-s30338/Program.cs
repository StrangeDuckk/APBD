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
            // ------------------ Stworzenie Kontenerowca ------------------
            var kontenerowiec = new Kontenerowiec(16,3828,10000);
            // ------------------ Stworzenie kontenera danego typu ------------------
            kontenerowiec.StworzenieKontenera("C", 10, 20, 150, 1500);
            kontenerowiec.StworzenieKontenera("G", 10, 20, 200, 1000);
            kontenerowiec.StworzenieKontenera("L", 10, 20, 100, 2000);
            kontenerowiec.StworzenieKontenera("M", 10, 20, 150, 1500);//takiego rodzaju kontenera nie ma
            kontenerowiec.StworzenieKontenera("G", 10, 20, 150, 1000);
            kontenerowiec.StworzenieKontenera("L", 10, 20, 150, 2000);
            kontenerowiec.StworzenieKontenera("C", 10, 20, 100, 1500);
            kontenerowiec.StworzenieKontenera("G", 10, 20, 250, 1000);
            kontenerowiec.StworzenieKontenera("L", 10, 20, 300, 2000);

            // ------------------ Załadowanie ładunku do danego kontenera ------------------
            
            // ------------------ Załadowanie kontenera na statek ------------------
            // ------------------ Załadowanie listy kontenerów na statek ------------------
            // ------------------ Usunięcie kontenera ze statku ------------------
            // ------------------ Rozładowanie kontenera ------------------
            // ------------------ Zastąpienie kontenera na statku o danym numerze innym kontenerem ------------------
            // ------------------ Możliwość przeniesienie kontenera między dwoma statkami ------------------
            // ------------------ Wypisanie informacji o danym kontenerze ------------------
            // ------------------ Wypisanie informacji o danym statku i jego ładunku ------------------
        }
    }
}
