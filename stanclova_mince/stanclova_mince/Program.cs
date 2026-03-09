using System.Collections.Generic;

namespace stanclova_mince
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] radky = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries); // StringSplitOptions.RemoveEmptyEntries - odebere prázdné mezery... a vubec je nebude přidávat do string[]
            List<int> seznamMince = new List<int>(); //pomocný list, abych do něj mohla přidávat rádky, které dám do intu
            foreach (string radek in radky)
            {
                seznamMince.Add(int.Parse(radek));
            }

            int[] mince = seznamMince.ToArray(); //výsledný list dám do seznamu

            int suma = int.Parse(Console.ReadLine()); //string do intu

            if (suma == 0)
            {
                Console.WriteLine("Nepoužije se žádná mince.");
            }

            //pomocné proměnné
            int indexMince = 0;
            List<List<int>> vsechnaReseni = new List<List<int>>();
            List<int> aktualniReseni = new List<int>();

            Backtracking(suma, indexMince, mince, aktualniReseni, vsechnaReseni);

            foreach (var reseni in vsechnaReseni)
            {
                Console.WriteLine(string.Join(" ", reseni));
            }
        }


        static void Backtracking(int zbyvaDoplnit, int indexMince, int[] mince, List<int> aktualniReseni, List<List<int>> vsechnaReseni)
        {
            //nalezli jsme řešení - daná větev je hotová
            if (zbyvaDoplnit == 0)
            {
                vsechnaReseni.Add(new List<int>(aktualniReseni)); //přidám toto konkrétní řešení do listu všech řešení
                                                                  //přidávám ale NOVÝ list - protože list ukládá referenční okdazy... vypisovaly by se jen prázdné řádky
                                                                  //zároveň jsem ti to na konci mazala - odkazy na prázdné senzmay 
                                                                  //nový list vezme všechna čísla v aktualnim řešení a nasype je do vsech řešení - nemaou se mi na konci
                return; //tím ukončím konkrétní větev rekurze - ne celý program - takže se vrátím hezky zpět a můžu zkoušet další větve
            }

            for (int i = indexMince; i < mince.Length; i++) //dám to na indexMince - tím projdu všechny mince a nebudu se znovu vracet k již prošlým
            {
                int hodnotaMince = mince[i];

                if (zbyvaDoplnit >= hodnotaMince)
                {
                    aktualniReseni.Add(mince[i]); //přidám minci do aktuálního řešení větve, kterou řeším a pošlu rekurzivně dál zpracovat
                    Backtracking(zbyvaDoplnit - hodnotaMince, i, mince, aktualniReseni, vsechnaReseni); //posílám index i, abych se nevracela k větším mincím

                    //pakliže se to neukončí ta větev, musím odebrat poslední minci - abych mohla zkoušet další vštve
                    aktualniReseni.RemoveAt(aktualniReseni.Count - 1); //když v aktuálním řešení budou 4 mince - dount 4 - odeberu minci s indexem 3 = poslední mince
                }
            }
        }
    }
}
