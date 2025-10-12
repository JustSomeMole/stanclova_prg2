using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stanclova_vztahy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Zadejte počet žen: ");
                    int pocetZen = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    LidiVztahy lidiVytahy = new LidiVztahy(pocetZen);

                    lidiVytahy.NacteniPreference();

                    // lidiVytahy.Vypis();

                    
                    for (int i = 1; i < pocetZen + 1; i++)
                    {
                        lidiVytahy.NajdiMuze(i);
                    }

                    Console.WriteLine();
                    lidiVytahy.VypisParecku();
            
                    break;
                }

                catch
                {
                    Console.WriteLine("Zadejte číslo!");
                }
                
            }

            Console.ReadLine();
        }
    }

    class LidiVztahy
    {
        private int[,] zenyPreference { get; set; }
        private int[,] muziPreference { get; set; }
        private int pocetZen { get; set; }
        private int radekNaVymazani { get; set; }

        private int[] partnerkyMuzu { get; set; }

        public LidiVztahy(int pocetZen)
        {
            this.pocetZen = pocetZen;
            zenyPreference = new int[pocetZen, pocetZen];
            muziPreference = new int[pocetZen, pocetZen];
            radekNaVymazani = 0;
            partnerkyMuzu = Enumerable.Repeat(0, pocetZen).ToArray(); //vytvořím si array intu, kam dám zatím -1 a to tolik, kolik je žen
                                                                      //sem budu zapisovat s kým je muž a porovnávat s lepší/horší nabídkou
                                                                      //INDEX JE MUZ, TO NA POZICI INDEXU JE ZENA
        }


        public void Vypis()
        {
            Console.WriteLine("Ženy prference:");
            for (int i = 0; i < pocetZen; i++) //počet řádků ... [[1, 2, 3],[4, 5, 6],[7, 8, 9]] --> délka je 3
            {
                for (int j = 0; j < pocetZen; j++)
                {
                    Console.Write(zenyPreference[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Muži prference:");
            for (int i = 0; i < pocetZen; i++) //počet řádků ... [[1, 2, 3],[4, 5, 6],[7, 8, 9]] --> délka je 3
            {
                for (int j = 0; j < pocetZen; j++)
                {
                    Console.Write(muziPreference[i, j].ToString());
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }


        public void NacteniPreference() // [[1, 2, 3],[4, 5, 6],[7, 8, 9]]
        {
            string[] radekPreference;
            int cislo;

            Console.WriteLine("Vlož svou matici:");


            for (int i = 0; i < pocetZen; i++)
            {
                radekPreference = Console.ReadLine().Split(' ');
                for (int j = 0; j < pocetZen; j++)
                {
                    cislo = Convert.ToInt32(radekPreference[j]);
                    zenyPreference[i, j] = cislo;
                }
            }


            for (int i = 0; i < pocetZen; i++)
            {
                radekPreference = Console.ReadLine().Split(' ');
                for (int j = 0; j < pocetZen; j++)
                {
                    cislo = Convert.ToInt32(radekPreference[j]);
                    muziPreference[i, j] = cislo;
                }
            }
        }

        /*
        public void VymazRadek(int radekNaVymazani, int[,] matice) //nahradí řádek nulami
        {
            for (int i = 0; i < pocetZen; i++)
            {
                matice[radekNaVymazani, i] = 0;
            }
        }
        */

        public void NajdiMuze(int zena)
        {
            int muz = 0;
            int muzPartnerka = 0;
            for (int i = 0; i < pocetZen; i++) //nasli jsme prnviho preferovaneho pro zenu na indexu zena
            {
                if (zenyPreference[zena - 1, i] != 0)
                {
                    muz = zenyPreference[zena - 1, i]; 
                    break;
                }
            }
            if (muz == 0) //jen pro jistotu ... mělo by nastat jen když je zadání blbě
            {
                return;
            }

            //v proměnné muz máme nejrpveferovanějsího volného muže
            //zjistíme jestli muž "muž" už tvoří s někým pár
            if (partnerkyMuzu[muz - 1] != 0) //s nekym tvori
            {
                muzPartnerka = partnerkyMuzu[muz - 1]; //toto je soucasna partnerka toho muze

                //ted se podivam koho preferuje vic
                //budeme rozhodovat mezi muzPartnerka a zena
                for (int i = 0; i < pocetZen; i++)
                {
                    if (muziPreference[muz - 1, i] == muzPartnerka) //preferuje toho s kym je nyni v paru
                    {
                        for (int j = 0; j < pocetZen; j++) //v preferencim seznamu zeny zena vymazeme toho muze (muz je v paru s nekym preferovanejsim)
                        {
                            if (zenyPreference[zena - 1, j] == muz)
                            {
                                zenyPreference[zena - 1, j] = 0;
                            }
                        }
                        
                        NajdiMuze(zena); //hledame jineho muze pro tuto zenu
                        return; //partner nalezen
                    }

                    if (muziPreference[muz - 1, i] == zena) //preferuje vice zenu "zena" nez se kterou byl v paru (partnerkaZena)
                    {
                        partnerkyMuzu[muz - 1] = zena; //muz m se zasnoubi se zenou zena

                        for (int j = 0; j < pocetZen; j++) //v preferencim seznamu byvale partnerky muze vymazeme toho muze 
                        { 
                            if (zenyPreference[muzPartnerka - 1, j] == muz) 
                            {
                                zenyPreference[muzPartnerka - 1, j] = 0;
                            }
                        }
                        
                        NajdiMuze(muzPartnerka); //pro tuto zenu nalezneme jineho muze ... ted je volna
                        return;
                    }
                }
            }

            else //s nikym netvori par, rovnou ho priradime
            {
                partnerkyMuzu[muz - 1] = zena; //zena je sparovana
            }
        }


        public void VypisParecku()
        {
            /* Console.WriteLine("M - Ž");
            for (int i = 0; i < pocetZen; i++)
            {
                Console.Write(i + 1);
                Console.Write(" - ");
                Console.WriteLine(partnerkyMuzu[i]);
            }
            */
            int[] pole = Enumerable.Repeat(0, pocetZen).ToArray();

            for (int i = 0; i < pocetZen; i++)
            {
                pole[partnerkyMuzu[i] - 1] = i + 1;
            }

            Console.WriteLine("Žena - Muž");
            for (int i = 0; i < pocetZen; i++)
            {
                Console.Write(i + 1);
                Console.Write(" - ");
                Console.WriteLine(pole[i]);
            }
        }
    }
}