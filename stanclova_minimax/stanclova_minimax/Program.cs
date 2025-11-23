using System;
using System.Collections.Generic;
using System.Linq;

namespace stanclova_minimax
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialPiles = new List<int> { 2, 2 };
            bool botStarts = true;

            var game = new NimGame(initialPiles, botStarts);
            GameState state;

            do
            {
                state = game.PlayTurn();
            } while (state == GameState.Ongoing);


            if (state == GameState.BotWon)
                Console.WriteLine("Vyhrál počítač!");
            else
                Console.WriteLine("Gratulujeme! Vyhráli jste!");
        }
    }

    public enum GameState
    {
        Ongoing,
        BotWon,
        HumanWon
    }

    public class NimGameState
    {
        public List<int> Piles { get; private set; }
        public int MatchesInGame { get; private set; }

        public NimGameState(List<int> initialPiles)
        {
            Piles = new List<int>(initialPiles);
            MatchesInGame = Piles.Sum();
        }

        public void MakeMove(int pileIndex, byte matchesToRemove)
        {
            if (IsValidMove(pileIndex, matchesToRemove))
            {
                Piles[pileIndex] -= matchesToRemove;
                MatchesInGame -= matchesToRemove;
            }
            else
            {
                throw new ArgumentException("Neplatný tah!");
            }
        }

        private bool IsValidMove(int pileIndex, byte matchesToRemove)
        {
            return pileIndex >= 0 &&
                   pileIndex < Piles.Count &&
                   Piles[pileIndex] >= matchesToRemove &&
                   matchesToRemove > 0;
        }
    }

    public class NimGame
    {
        private NimGameState _state; // pro privátní datové položky používáme podtržítko na začátku jména
        private bool _botStarts;
        private bool _isBotTurn;

        public NimGame(List<int> initialPiles, bool botStarts)
        {
            _state = new NimGameState(initialPiles);
            _botStarts = botStarts;
            _isBotTurn = botStarts;
        }

        public GameState PlayTurn()
        {
            PrintGameState();

            if (_isBotTurn)
            {
                var botMove = GetBestBotMove();
                MakeAndPrintBotMove(botMove);
            }
            else
            {
                var humanMove = GetHumanInput();
                _state.MakeMove(humanMove.Item1, humanMove.Item2);
            }

            _isBotTurn = !_isBotTurn;

            if (_state.MatchesInGame == 0)
                if (_isBotTurn)
                    return GameState.BotWon;
                else
                    return GameState.HumanWon;
            else
                return GameState.Ongoing;
        }

        private Tuple<int, byte> GetBestBotMove()
        {
            int bestPile = 0; // budoucí nejlepší hromádka k odebírání sirek
            byte matchesToRemove = 1; // buoducí nejlepší momentální počet k odebrání

            int score = minimax(_state.Piles.ToList(), 10, true);

            int best;

            int minimax(List<int> piles, int depth, bool maximizingPlayer)
            {
                if (piles.Sum() == 0) //konec - nemám co procházet
                {
                    if (maximizingPlayer == true) //bot na tahu = bot prohrál
                    {
                        return 1;
                    }
                    else 
                    {
                        return -1;
                    }
                }

                if (maximizingPlayer == true) //bot ... chce max
                {
                    best = int.MinValue;

                    for (int i = 0; i < piles.Count; i++) //procházím jednotlivé hromádky
                    {
                        for (byte remove = 1; remove <= Math.Min(2, piles[i]) /*zkusím buď odebrat 2 nebo max sirek v hromádce, vybere to to menší číslo*/; remove++) //zkusím odebrat 1 a pak 2 sirky
                        {
                            var newPile = piles.ToList();

                            newPile[i] -= remove; //odeberu sirku

                            score = minimax(newPile, depth-1, !maximizingPlayer);

                            if (score > best) //našla jsem něco lepšího
                            {
                                best = score;
                                bestPile = i;
                                matchesToRemove = remove;
                            }
                        } 
                    }
                    return best;
                }

                else //clovek ... chce min
                {
                    best = int.MaxValue;

                    for (int i = 0; i < piles.Count; i++) //procházím jednotlivé hromádky
                    {
                        for (byte remove = 1; remove <= Math.Min(2, piles[i]) /*zkusím buď odebrat 2 nebo max sirek v hromádce, vybere to to menší číslo*/; remove++) //zkusím odebrat 1 a pak 2 sirky
                        {
                            var newPile = piles.ToList();

                            newPile[i] -= remove; //odeberu sirku

                            score = minimax(newPile, depth-1, !maximizingPlayer);

                            if (score < best) //našla jsem něco lepšího
                            {
                                best = score;
                                bestPile = i;
                                matchesToRemove = remove;
                            }
                        }
                    }
                    return best;
                }
            }

            return new Tuple<int, byte>(bestPile, matchesToRemove);
        }

        private void PrintGameState()
        {
            Console.WriteLine("Aktuální stav hry:");
            foreach (var pile in _state.Piles)
                Console.Write(pile + " ");
            Console.WriteLine();
        }

        private void MakeAndPrintBotMove(Tuple<int, byte> move)
        {
            _state.MakeMove(move.Item1, move.Item2);
            Console.WriteLine($"Počítač bere {move.Item2} sirky z hromádky {move.Item1}");
        }

        private Tuple<int, byte> GetHumanInput()
        {

            Console.Write("Z které hromádky chcete brát? (");
            for (int i = 0; i < _state.Piles.Count; i++)
            {
                if (_state.Piles[i] > 0)
                    Console.Write($"{i} ");
            }
            Console.Write(")");

            int pileIndex = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine($"Kolik sirek chcete vzít? (1-{_state.Piles[pileIndex]})");
            byte matches = Convert.ToByte(Console.ReadLine());

            return new Tuple<int, byte>(pileIndex, matches);
        }
    }


}