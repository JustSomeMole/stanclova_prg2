using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace stanclova_uvodni_hodina
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //---VYTVOŘENÍ FILMŮ (instancekonkrétní objekt vytvořený z té třídy ... přijímá její parametry...)---//
            Film film1 = new Film() //tohle mi dovolí to přidat vše najednou, oddělení čárkami, středník je až na konci, nemusím furt psát film1.Nazev = něco
            {
                Nazev = "Marťan",
                RokVzniku = 2015,
                JmenoRezisera = "Ridley",
                PrijmeniRezisera = "Scott"

            };

            Film film2 = new Film()
            {
                Nazev = "Strážci galaxie",
                RokVzniku = 2014,
                JmenoRezisera = "James",
                PrijmeniRezisera = "Gunn"
            };

            Film film3 = new Film()
            {
                Nazev = "Hledá se Nemo",
                RokVzniku = 2003,
                JmenoRezisera = "Andrew", //dala jsem jen jednoho režiséra
                PrijmeniRezisera = "Stanton"
            };


            List<Film> seznamFilmu = new List<Film>
            {
                film1,
                film2,
                film3
            };


            Random rnd = new Random(); //toto teď umí generovat náhodná čísla
            int noveHodnoceni;
            foreach (var film in seznamFilmu) //var = nechám kompilátor odvodit, co to je
            {
                for (int i = 0; i < 15; i++)
                {
                    noveHodnoceni = rnd.Next(6); //čísla od 0 do 5; metoda Next žádá o další číslo
                    film.PridaniHodnoceni(noveHodnoceni);
                }
            }

            //---ÚVOD---//
            Console.WriteLine("VÍTEJTE U NAŠEHO KAŽDOVEČERNÍHO A CELOVEČERNÍHO POŘADU NEROZUMIMEFILMUMALEHODNOTIMEJE!\n---");

            System.Threading.Thread.Sleep(1500); //v ms ... toto je 1,5s ... program počká ... hezký efekt jakože loadingu ... source: https://www.youtube.com/watch?v=sP8iD0eMt48

            //---HODNOCENÍ, ...---//
            double prumerneHodnoceni = 0;
            string nazevNejFilmu = "";
            string nazevNejdelsihoNazvuFilmu = "";
            int nejPocetPismenVNazvu = 0;
            int pocetPismenVNazvu = 0;
            foreach (var film in seznamFilmu)
            {
                pocetPismenVNazvu = film.Nazev.Length; //i s mezerami

                //hodnoceni
                if (film.Hodnoceni > prumerneHodnoceni)
                {
                    prumerneHodnoceni = film.Hodnoceni;
                    nazevNejFilmu = film.Nazev;
                }
                else if (film.Hodnoceni == prumerneHodnoceni)
                {
                    nazevNejFilmu += " společně s " + film.Nazev;
                }

                //delka nazvu
                if (pocetPismenVNazvu > nejPocetPismenVNazvu)
                {
                    nejPocetPismenVNazvu = pocetPismenVNazvu;
                    nazevNejdelsihoNazvuFilmu = film.Nazev;
                }

                else if (pocetPismenVNazvu == prumerneHodnoceni)
                {
                    nazevNejdelsihoNazvuFilmu += " společně s " + film.Nazev;
                }
            }
            Console.WriteLine("Filmem s nejlepším hodnocením se stal: " + nazevNejFilmu + "! " + "A filmem s nejdelším názvem se stal: " + nazevNejdelsihoNazvuFilmu + "!\n---");

            System.Threading.Thread.Sleep(1500);

            //---ŠPATNÉ FILMY---//
            Console.OutputEncoding = System.Text.Encoding.UTF8; //přidáno abych mohla vypisovat hvězdičky... princip: říká konzoli aby při výpisu znaků používala UTF-8 kódování, takže se správně zobrazí česká písmena a speciální znaky .. ChatGPT velmi pomohl, nevěděla jsem, jak to udělat... 
            Console.WriteLine("HŘÍŠNÍCI S MALÝM HODNOCENÍM\n");
            foreach (var film in seznamFilmu)
            {
                if (film.Hodnoceni < 3)
                {
                    Console.WriteLine("Film " + film.Nazev + " je odpad! Má hodnocení jen " + film.Hodnoceni.ToString("0.00") + "\u2605!!"); //aby to bylo jen na 2 desetinné
                    System.Threading.Thread.Sleep(1000);
                }
            }
            Console.WriteLine("---");

            System.Threading.Thread.Sleep(1500);

            //---VŠECHNY FILMY---//
            Console.WriteLine("A na závěr účastníci našeho dnešního pořadu byli:");
            foreach (var film in seznamFilmu)
            {
                film.VypsaniHodnoceni();
                System.Threading.Thread.Sleep(1000);
            }



            Console.ReadLine();
        }
    }

    class Film
    {
        public string Nazev;            
        public string JmenoRezisera;
        public string PrijmeniRezisera;
        public int RokVzniku;

        public double Hodnoceni { get; private set; }

        public List<int> TabulkaHodnoceni = new List<int>();


        //---FUNKCE---//
        public void PridaniHodnoceni(int NoveHodnoceni)
        {
            TabulkaHodnoceni.Add(NoveHodnoceni);
            VytvoreniHodnoceni(); //rovnou vytvořím průměr
        }

        private void VytvoreniHodnoceni()
        {
            double PocetPrvku = TabulkaHodnoceni.Count; //rovnou nechám double, aby to bylo přesné a ne na celé čísla
            double Soucet = 0;

            for (int i = 0; i < PocetPrvku; i++)
            {
                Soucet += TabulkaHodnoceni[i];
            }

            Hodnoceni = Soucet / PocetPrvku;
        }

        public override string ToString()
        {
            string filmInformace = Nazev + " (" + RokVzniku + "; " + PrijmeniRezisera + ", " + JmenoRezisera[0] + "): " + Hodnoceni.ToString("0.00") + "\u2605";
            return filmInformace;
        }

        public void VypsaniHodnoceni()
        {
            string vypis = ToString();
            Console.WriteLine(vypis); //rovnou zavolám, uložím a vypíšu
        }
    }
}

