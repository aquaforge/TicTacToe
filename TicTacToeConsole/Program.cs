using TicTacToeGame;

Game g = new(25, 20);
//PlayerMinMax x = new(PlayerTypes.X);
//PlayerMinMax y = new(PlayerTypes.Y);

int counter = 0;


while (true)
{
    g.DoMove(new Point(counter,counter));
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
                    s = "x";
                    break;
                case PlayerTypes.O:
                    s = "o";
                    break;
                default:
                    break;
            }
            if (lastMovePoint.M == m && lastMovePoint.N == n) s = s.ToUpper();

            Console.Write(s + " ");
        }
        Console.WriteLine();
    }
    Console.WriteLine(g.GameState);

    Console.ReadKey();

}

