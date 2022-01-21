using System.Text;
using TicTacToeGame;

namespace TicTacToeGame
{
    public class TicTacToeConcole
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                DoOneGame(i);
                Thread.Sleep(1000);
            }


            Console.ReadKey();
        }
        public static void DoOneGame(int gameNo = 0)
        {
            GeneralFileLogger.Log("Start", true);

            IGame g = new Game(10, 8);
            IPlayerAI playerMinMax = new PlayerMinMax();

            string filename = @"D:\Temp\TicTacToe\" + $"games_{g.LengthRow:00}_{g.LengthCol:00}.log";

            while (!g.IsGameFinished())
            {
                g.DoStartThinking();
                Point next = playerMinMax.GetNextMovement(g);
                SaveMovement(g, next);
                g.DoMove(next);
                DrawToConsole(g, gameNo);
            }
            File.AppendAllText(filename, g.ToString() + Environment.NewLine);
        }

        private static void SaveMovement(IGame g, Point next)
        {
            StringBuilder sb = new();
            string filename = @"D:\Temp\TicTacToe\" + $"movements_{g.LengthRow:00}_{g.LengthCol:00}.csv";

            if (!File.Exists(filename))
            {
                sb.Append("class;");
                for (int row = 0; row < g.LengthRow; row++)
                    for (int col = 0; col < g.LengthCol; col++)
                        sb.Append($"item{row:00}{col:00};");
                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
                File.WriteAllText(filename, sb.ToString());
                sb.Clear();
            }

            sb.Append(next.Row * g.LengthCol + next.Col);
            sb.Append(';');

            for (int row = 0; row < g.LengthRow; row++)
                for (int col = 0; col < g.LengthCol; col++)
                {
                    switch (g[row, col])
                    {
                        case PlayerTypes.EMPTY:
                            sb.Append(0);
                            break;
                        case PlayerTypes.X:
                        case PlayerTypes.O:
                            sb.Append(g[row, col] == g.PlayerToMove ? 1 : -1);
                            break;
                        default:
                            break;
                    }
                    sb.Append(';');
                }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);
            File.AppendAllText(filename, sb.ToString());
        }

        private static void DrawToConsole(IGame g, int gameNo = 0)
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
                    Console.Write(" ");
                    if (MovementLastCell.Row == row && MovementLastCell.Col == col)
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(g.CellAsString(row, col));
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }

            //Draw Info
            var sb = new StringBuilder();
            if (gameNo > 0) sb.Append($"gameNo={gameNo} ");
            sb.Append($"{g.GameState} Movements={g.MovementsCount()} ");

            List<Movement> movements = g.MovementsGetTop();
            foreach (PlayerTypes pt in new[] { PlayerTypes.X, PlayerTypes.O })
            {
                var playerMovements = movements.Where(m => m.Player == PlayerTypes.X && m.ElapsedMilliseconds > 0);
                if (playerMovements.Any())
                    sb.Append($"{pt}: {(int)playerMovements.Average(m => m.ElapsedMilliseconds)}ms ");
            }
            sb.AppendLine();
            for (int i = movements.Count - 1; i >= Math.Max(0, movements.Count - 5); i--)
                sb.AppendLine(movements[i].ToString());

            Console.WriteLine(sb.ToString());
        }


    }
}









