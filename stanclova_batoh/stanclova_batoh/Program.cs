using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace stanclova_batoh
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
            //NAČTENÍ CEN -------
            string[] radek1 = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.RemoveEmptyEntries - odebere prázdné mezery... a vubec je nebude přidávat do string[]
            List<int> seznamCen = new List<int>(); //pomocný list, abych do něj mohla přidávat rádky, které dám do intu
            foreach (string cislo1 in radek1)
            {
                seznamCen.Add(int.Parse(cislo1));
            }

            int[] ceny = seznamCen.ToArray(); //výsledný list dám do seznamu


            //NAČTENÍ VAH -------
            string[] radek2 = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries); 
            List<int> seznamVah = new List<int>(); 
            foreach (string cislo2 in radek2)
            {
                seznamVah.Add(int.Parse(cislo2));
            }

            int[] vahy = seznamVah.ToArray();


            //KAPACITA BATOHU -------
            int kapacitaBatohu = int.Parse(Console.ReadLine()); //string do intu

            if (kapacitaBatohu == 0)
            {
                Console.WriteLine("Batoh bude prázdný.");
            }
            */

            //načtení ze souboru
            string[] vsechnyRadky = File.ReadAllLines("Knapsack_testy.txt");

            for (int i = 6; i < vsechnyRadky.Length; i += 6) //projdu celý program a postupně vypíšu jednotlivá řešení
                                                             //je to vždycky blok po 5 řádcích s 1 řádkem jako mezera
            {
                string[] radek1 = vsechnyRadky[i].Split(' ', StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.RemoveEmptyEntries - odebere prázdné mezery... a vubec je nebude přidávat do string[]
                List<int> seznamCen = new List<int>(); //pomocný list, abych do něj mohla přidávat rádky, které dám do intu
                foreach (string cislo1 in radek1)
                {
                    seznamCen.Add(int.Parse(cislo1));
                }
                int[] ceny = seznamCen.ToArray(); //výsledný list dám do seznamu

                string[] radek2 = vsechnyRadky[i + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                List<int> seznamVah = new List<int>();
                foreach (string cislo2 in radek2)
                {
                    seznamVah.Add(int.Parse(cislo2));
                }
                int[] vahy = seznamVah.ToArray();

                int kapacita = int.Parse(vsechnyRadky[i + 2]);



                string ocekavanyProfit = vsechnyRadky[i + 3];
                string ocekavanyVysledek = vsechnyRadky[i + 4];



                //pomocné proměnné
                int nejvetsiCena = 0;
                List<(int cena, int vaha, int index)> nejlepsiReseni = new List<(int, int, int)>(); //budu to ukládat jako dvojice čísel - cena, vaha ... tuple
                List<(int cena, int vaha, int index)> aktualniReseni = new List<(int, int, int)>();

                Backtracking(kapacita, 0, ref nejvetsiCena, 0, ceny, vahy, aktualniReseni, nejlepsiReseni);




                Console.WriteLine($"-------------");
                Console.WriteLine($"Vypočítaný profit: {nejvetsiCena} (Očekávaný: {ocekavanyProfit})");


                Console.Write("Předměty: ");

                foreach (var reseni in nejlepsiReseni)
                {
                    Console.Write((reseni.index + 1) + " "); //ukládám indexy ne pořadí - přidám +1 abych měla pořadí
                }

                Console.WriteLine($"Očekávaný výsledek: {ocekavanyVysledek}");
                Console.WriteLine("-------------");

                Console.WriteLine();
            }
        }


        static void Backtracking(int zbyvajiciMisto, int indexPredmetu, ref int nejvetsiCena, int aktualniCena, int[] ceny, int[] vahy, List<(int cena, int vaha, int index)> aktualniReseni, List<(int cena, int vaha, int index)> nejlepsiReseni)
        {
            //nalezli jsme řešení - daná větev je hotová
            //kontrluji to pokaždé - když najdu lepší řešení, uložím ho
            if (aktualniCena > nejvetsiCena) //našla jsem lepší řešení
            {
                nejvetsiCena = aktualniCena;
                nejlepsiReseni.Clear(); //vyčistím staré nejlepší reseni
                nejlepsiReseni.AddRange(aktualniReseni); //ulozím do nej aktualni reseni - postupne ulozim vsechny hodnoty v akrualnim reseni
            }

            //ve všech případech ukončím větev - i když jsem nenašla nejlepší řešení - nalezla jsem nějaké řešení
            //return; //tím ukončím konkrétní větev rekurze - ne celý program - takže se vrátím hezky zpět a můžu zkoušet další větve

            for (int i = indexPredmetu; i < ceny.Length; i++) //dám to na indexMince - tím projdu všechny mince a nebudu se znovu vracet k již prošlým
            {
                int cenaPredmetu = ceny[i];
                int vahaPredmetu = vahy[i];

                if (zbyvajiciMisto >= vahaPredmetu)
                {
                    aktualniReseni.Add((cenaPredmetu, vahaPredmetu, i));
                    Backtracking(zbyvajiciMisto - vahaPredmetu, i + 1 /*každý předmět 1*/, ref nejvetsiCena, aktualniCena + cenaPredmetu, ceny, vahy, aktualniReseni, nejlepsiReseni); 

                    //pakliže se to neukončí ta větev, musím odebrat poslední minci - abych mohla zkoušet další vštve
                    aktualniReseni.RemoveAt(aktualniReseni.Count - 1); //když v aktuálním řešení budou 4 mince - dount 4 - odeberu minci s indexem 3 = poslední mince
                }
            }
        }
    }
}