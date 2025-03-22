﻿using System;
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
        public Kontenerowiec(Kontener[] kontenery, double maksymalnaPredkosc, int maksymalnaLiczbaKontenerow, double maksymalneObciazenie)
        {
            this.kontenery = new List<Kontener>();
            this.maksymalnaPredkosc = maksymalnaPredkosc;
            this.maksymalnaLiczbaKontenerow = maksymalnaLiczbaKontenerow;
            this.maksymalneObciazenie = maksymalneObciazenie;
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
        
    }
}
