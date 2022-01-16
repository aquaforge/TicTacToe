using TicTacToeGame;

Game g = new(25, 20);
//PlayerMinMax x = new(PlayerTypes.X);
//PlayerMinMax y = new(PlayerTypes.Y);

int counter = 0;


while (true)
{
    g.DoMove(new Point(counter, counter));
    counter++;


    Console.Clear();
    Point lastMovePoint = g.LastMovePoint;
    for (int n = 0; n < g.LengthN; n++)
    {
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
            if (lastMovePoint.M == m && lastMovePoint.N == n) Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(s);
            Console.ResetColor();
            Console.Write(" ");
        }
        Console.WriteLine();
    }
    Console.WriteLine(g.GameState);

    Console.ReadKey();

}

