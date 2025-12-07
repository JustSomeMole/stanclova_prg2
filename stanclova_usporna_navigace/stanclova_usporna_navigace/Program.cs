using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace stanclova_usporna_navigace
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string soubor = Path.Combine(AppContext.BaseDirectory, "in_1.txt");
            Navigace navigace = new Navigace();
            int[,] graf = navigace.NactiData(soubor);

            int radky = graf.GetLength(0);
            int sloupce = graf.GetLength(1);

            for (int i = 0; i < radky; i++)
            {
                for (int j = 0; j < sloupce; j++)
                {
                    Console.Write(graf[i, j] + "\t"); // \t = tabulátor pro lepší zarovnání
                }
                Console.WriteLine();
            }
        }

        class Navigace
        {
            public int Start;
            public int Cil;
            /// <summary>
            /// Graf měst bude mít vždy pevně 4 položky jako počet sloupců a počet řádků bude záviset na počtu silnic
            /// </summary>
            public int[,] Graf_mest;
            public PriorityQueue<int, int> prioritni_fronta = new PriorityQueue<int, int>(); //inicializuji rovnou
            public int[] vzdalenost;
            public int[] cesta;

            public Navigace() { }

            public int[,] NactiData(string soubor)
            {
                using (StreamReader sr = new StreamReader(soubor))
                {
                    //první řádek
                    string prvniRadek = sr.ReadLine();
                    string[] prvniRadek_cast = prvniRadek.Split(' ');

                    int pocetMest = int.Parse(prvniRadek_cast[0]);
                    int pocetSilnic = int.Parse(prvniRadek_cast[1]);

                    InicializujVzdalenosti(pocetMest);

                    Graf_mest = new int[pocetSilnic, 4];

                    for (int i = 0; i < pocetSilnic; i++)
                    {
                        string radek = sr.ReadLine();
                        string[] data_radku = radek.Split(' ');

                        for (int j = 0; j < data_radku.Length; j++)
                        {
                            int data_radku_cislo = int.Parse(data_radku[j]);
                            Graf_mest[i, j] = data_radku_cislo;
                        }
                    }
                    return Graf_mest;
                }
            }

            public void InicializujVzdalenosti(int pocetMest)
            {
                vzdalenost = new int[pocetMest];
                cesta = new int[pocetMest];
                for (int i = 0; i < pocetMest; i++)
                {
                    vzdalenost[i] = int.MaxValue;
                }
            }

            public void Pruchod()
            {
                vzdalenost[Start] = 0;
                cesta[Start] = -1;
                prioritni_fronta.Enqueue(Start, 0);

                int kontrolujici_mesto = Start;

                while (prioritni_fronta.Count > 0)
                {
                    kontrolujici_mesto = prioritni_fronta.Dequeue(); //první prvek z fronty - odeberu ho

                    if (kontrolujici_mesto == Cil)
                        break; //našla jsem cíl

                    for (int i = 0; i < Graf_mest.GetLength(0); i++)
                    {
                        if (Graf_mest[i, 0] == kontrolujici_mesto)
                        {
                            int sousedni_mesto = Graf_mest[i, 1];
                            int vzdalenost_mest = Graf_mest[i, 2];
                            int nova_vzdalenost = vzdalenost[kontrolujici_mesto] + vzdalenost_mest;

                            if (nova_vzdalenost < vzdalenost[sousedni_mesto])
                            {
                                vzdalenost[sousedni_mesto] = nova_vzdalenost; //sečtu to se vzdáleností, která tam již byla

                                prioritni_fronta.Enqueue(sousedni_mesto, nova_vzdalenost); //přidám do fronty město koncové a vzdálenost jeho od startu   

                                cesta[sousedni_mesto] = kontrolujici_mesto;
                            }
                        }
                    }
                }
            }

            public List<int> VypisCesty()
            {
                List<int> vysledek = new List<int>();

                int aktualni_bod = Cil;

                while (aktualni_bod != -1)
                {
                    vysledek.Add(aktualni_bod);
                    aktualni_bod = cesta[aktualni_bod];
                }

                vysledek.Reverse();

                return vysledek;
            }
        }
    }
}
