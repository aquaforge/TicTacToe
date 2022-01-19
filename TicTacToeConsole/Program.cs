using System.Text;
using TicTacToeGame;

namespace TicTacToeGame
{
    public class TicTacToeConcole
    {
        public static void Main(string[] args)
        {
            GeneralFileLogger.Log("Start", true);

            IGame g = new Game(20, 25);
            IPlayerAI playerMinMax = new PlayerMinMax();

            while (!g.IsGameFinished())
            {
                g.DoStartThinking();
                g.DoMove(playerMinMax.GetNextMovement(g));
                DrawToConsole(g);
                //Thread.Sleep(100);
            }
            Console.ReadKey();
        }

        private static void DrawToConsole(IGame g)
        {
            //Header
            Console.Clear();
            Console.Write(new string(' ', 3));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int col = 0; col < g.LengthCol; col++)
                Console.Write($"{col,2} ");
            Console.ResetColor();
            Console.WriteLine();


            //Draw Matrix
            Point MovementLastCell = g.MovementLastCell;
            for (int row = 0; row < g.LengthRow; row++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{row,2} ");
                Console.ResetColor();

                for (int col = 0; col < g.LengthCol; col++)
                {
                    string s = "_";
                    switch (g[row, col])
                    {
                        case PlayerTypes.X:
                            s = "X";
                            break;
                        case PlayerTypes.O:
                            s = "O";
                            break;
                        default:
                            break;
                    }
                    Console.Write(" ");
                    if (MovementLastCell.Row == row && MovementLastCell.Col == col)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(s);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            //Draw Info
            var sb = new StringBuilder();
            sb.Append(g.GameState.ToString().ToUpper() + " ");
            sb.Append($"Movements={g.MovementsCount()} ");

            List<Movement> movements = g.MovementsGetTop();
            foreach (PlayerTypes pt in new[] { PlayerTypes.X, PlayerTypes.O })
            {
                int elapsed = (int)movements.Where(m => m.Player == PlayerTypes.X && m.ElapsedMilliseconds > 0).Average(m => m.ElapsedMilliseconds);
                sb.Append($"{pt}: {elapsed}ms ");
            }
            sb.AppendLine();
            for (int i = movements.Count - 1; i >= Math.Max(0, movements.Count - 5); i--)
                sb.AppendLine(movements[i].ToString());

            Console.WriteLine(sb.ToString());
        }


    }
}









