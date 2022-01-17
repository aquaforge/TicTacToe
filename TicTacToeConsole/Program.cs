using TicTacToeGame;

Game g = new(25, 20);
IPlayerAI playerMinMax = new PlayerMinMax();

int counter = 0;
Random rand = new Random();


while (true)
{
    g.DoStartThinking();
    g.DoMove(new Point(counter, counter));
    counter++;

    #region HEADER
    Console.Clear();
    Console.Write("   ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    for (int m = 0; m < g.LengthM; m++)
    {
        Console.Write($"{m,2} ");
    }
    Console.ResetColor();
    Console.WriteLine();
    #endregion

    Point MovementLastCell = g.MovementLastCell;

    for (int n = 0; n < g.LengthN; n++)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"{n,2} ");
        Console.ResetColor();

        for (int m = 0; m < g.LengthM; m++)
        {
            string s = "_";
            switch (g[m, n])
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
            if (MovementLastCell.M == m && MovementLastCell.N == n)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(s);
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();
    }

    Console.WriteLine(g.GameState.ToString().ToLower() + " " + g.MovementsCount);
    List<Movement> movements = g.MovementsGetTop;
    for (int i = movements.Count() - 1; i >= 0; i--)
        Console.WriteLine(movements[i].ToString());

    Console.ReadKey();
}

