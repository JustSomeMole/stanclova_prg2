using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace stanclova_beast_in_labyrinth
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
            
            Console.ReadLine();
        }
    }


    class Game
    {
        private Maze maze;
        private Beast beast;


        public void Run()
        {
            Console.WriteLine("Zadejte počet řádků: ");
            int sloupec = int.Parse(Console.ReadLine());
            Console.WriteLine("Zadejte počet sloupc;: ");
            int radek = int.Parse(Console.ReadLine());

            char[,] labyrint = NacteniLabyrintu(radek, sloupec);
            Console.WriteLine();

            maze = new Maze(radek, sloupec, labyrint);
            beast = new Beast(maze);

            for (int i = 1; i <= 20; i++)
            {
                beast.BeastPohyb();
                Console.WriteLine(i+". krok");
                maze.VypsaniLabyrintu();
                Console.WriteLine();
            }
        }


        //---POMOCNÁ FUNKCE---//
        public static char[,] NacteniLabyrintu(int radek, int sloupec) // [[1, 2, 3],[4, 5, 6],[7, 8, 9]]
        {
            char[,] labyrint = new char[radek, sloupec];
            string radekLabyrintu; //string je pole charů

            Console.WriteLine("Vlož svou matici:");

            for (int i = 0; i < radek; i++)
            {
                radekLabyrintu = Console.ReadLine();

                for (int j = 0; j < sloupec; j++)
                {
                    labyrint[i, j] = radekLabyrintu[j];
                }
            }

            return labyrint;
        }
    }


    class Maze
    {
        public char[,] Labyrint { get; private set; }
        public int Radek { get; }
        public int Sloupec { get; }

        public Maze(int radek, int sloupec, char[,] labyrint)
        {
            Labyrint = labyrint;
            Radek = radek;
            Sloupec = sloupec;
        }



        //---FUNKCE---//
        public void VypsaniLabyrintu()
        {
            for (int i = 0; i < Radek; i++) //počet řádků ... [[1, 2, 3],[4, 5, 6],[7, 8, 9]] --> délka je 3
            {
                for (int j = 0; j < Sloupec; j++)
                {
                    Console.Write(Labyrint[i, j]);
                }
                Console.WriteLine();
            }
        }        
    }



    class Beast
    {
        int[] PozicePrisery { get; set; }
        int[] PozicePriseryPred { get; set; }
        char ZnakPrisery { get; set; }
        string MozneZnakyPrisery { get; }

        int[,] Steps { get; }

        bool PredeslyKrokOtockaVlevo { get; set; }


        private Maze labyrint; //abya příšera viděla do stejného labyrintu

        public Beast(Maze labyrint)
        {
            this.labyrint = labyrint;

            PozicePrisery = new int[2];
            PozicePriseryPred = new int[2];

            Steps = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; //right, down, left, up

            MozneZnakyPrisery = ">v<^";

            NalezeniPozicePrisery(); //pořád budu hledat příšeru při zavolání classy beast

            PredeslyKrokOtockaVlevo = false;
        }



        //------FUNKCE------//
        private void NalezeniPozicePrisery()
        {
            for (int i = 0; i < labyrint.Radek; i++)
            {
                for (int j = 0; j < labyrint.Sloupec; j++)
                {
                    if (labyrint.Labyrint[i, j] == '>' | labyrint.Labyrint[i, j] == '<' | labyrint.Labyrint[i, j] == '^' | labyrint.Labyrint[i, j] == 'v') //labyrint.Labyrint tím řeknu, že chci, aby v objektu labyrint, příšera viděla samotný Labyrint
                    {
                        PozicePrisery[0] = i;
                        PozicePrisery[1] = j;
                        PozicePriseryPred[0] = i;
                        PozicePriseryPred[1] = j;
                        ZnakPrisery = labyrint.Labyrint[i, j];
                        return;
                    }
                }
            }
        }


        //---otočky---//
        private void TurnRight() //nastane v případě, že vedle mě není stěna
        {
            int indexZnaku = MozneZnakyPrisery.IndexOf(ZnakPrisery); //najde znak prisery

            if (indexZnaku == MozneZnakyPrisery.Length - 1) //pro možné přidání znaků ... jsem nakonci "přehupsu" na začátek
            {
                ZnakPrisery = MozneZnakyPrisery[0];
            }
            else
            {
                ZnakPrisery = MozneZnakyPrisery[indexZnaku + 1]; //postupuji doprava
            }

            labyrint.Labyrint[PozicePrisery[0], PozicePrisery[1]] = ZnakPrisery;
        }

        private void TurnLeft() //nastane v případě, že přede mnou je stěna
        {
            int indexZnaku = MozneZnakyPrisery.IndexOf(ZnakPrisery); //najde znak prisery

            if (indexZnaku == 0) //pro možné přidání znaků ... jsem na začátku a "přehupsu" na konec
            {
                ZnakPrisery = MozneZnakyPrisery[MozneZnakyPrisery.Length - 1]; //konec seznamu
            }
            else
            {
                ZnakPrisery = MozneZnakyPrisery[indexZnaku - 1]; //postupuji doleva
            }

            labyrint.Labyrint[PozicePrisery[0], PozicePrisery[1]] = ZnakPrisery;
        }


        //---kontroly---//
        private bool KontrolaMistaPredSebou() //true pokud prázdno, false pokud zeď
        {
            int indexZnaku = MozneZnakyPrisery.IndexOf(ZnakPrisery);

            if (labyrint.Labyrint[PozicePrisery[0] + Steps[indexZnaku, 0], PozicePrisery[1] + Steps[indexZnaku, 1]] == 'X') //přičtu k tomu číslo vektoru ve směru, kam se kouká - >v<^ ... {[0,1],[1,0],[0,-1],[-1,0]} ... vektory odpovídají pozici příšery kam se kouká
            {
                return false;
            }
            return true;
        }

        private bool StenaNapravo() //true pokud je, false pokud neni
        {
            int indexZnaku = MozneZnakyPrisery.IndexOf(ZnakPrisery);
            int osetrenyIndex = (indexZnaku + 1) % MozneZnakyPrisery.Length; //4 (to by přeteklo) % 3 = 1... už ok... např 2%3 = 2 ... zbytek 2

            int radek = PozicePrisery[0] + Steps[osetrenyIndex, 0];
            int sloupec = PozicePrisery[1] + Steps[osetrenyIndex, 1];

            if (radek < 0 || radek >= labyrint.Radek || sloupec < 0 || sloupec >= labyrint.Sloupec) //ošetřím jak za hranicí labyrintu (přetéká z libovolné strany)
            {
                return true; //beru jakože tam stěna je    
            }

            if (labyrint.Labyrint[radek, sloupec] == 'X')
            {
                return true;
            }
            return false;
        }


        //---krok---//
        private void Krok()
        {
            int indexZnaku = MozneZnakyPrisery.IndexOf(ZnakPrisery);

            PozicePriseryPred[0] = PozicePrisery[0];
            PozicePriseryPred[1] = PozicePrisery[1];
            
            PozicePrisery[0] += Steps[indexZnaku, 0];
            PozicePrisery[1] += Steps[indexZnaku, 1];

            labyrint.Labyrint[PozicePriseryPred[0], PozicePriseryPred[1]] = '.';
            labyrint.Labyrint[PozicePrisery[0], PozicePrisery[1]] = ZnakPrisery;
        }

        //------PROGRAM------//
        bool ProvedKrok = false;
        public void BeastPohyb()
        {
            if (ProvedKrok == true)
            {
                Krok();
                ProvedKrok = false;
                return;
            }

            else
            {
                if (StenaNapravo() == true & KontrolaMistaPredSebou() == true)
                {
                    Krok();
                    return;
                }

                if (StenaNapravo() == true & KontrolaMistaPredSebou() == false)
                {
                    TurnLeft();
                    return;
                }

                if (StenaNapravo() == false & KontrolaMistaPredSebou() == false)
                {
                    TurnRight();
                    ProvedKrok = true;
                    return;
                }

                if (StenaNapravo() == false)
                {
                    TurnRight();
                    ProvedKrok = true;
                    return;
                }
            }
        }
    }
}


