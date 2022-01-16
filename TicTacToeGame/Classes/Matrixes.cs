using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class Matrixes
    {

        static public T[,] MatrixCopy<T>(T[,] mtrx)
        {
            var b = new T[mtrx.GetUpperBound(0), mtrx.GetUpperBound(1)];
            for (int m = 0; m <= mtrx.GetUpperBound(0); m++)
                for (int n = 0; n <= mtrx.GetUpperBound(1); n++)
                    b[m, n] = mtrx[m, n];
            return b;

        }

        static public bool IsWon(PlayerTypes[,] board, PlayerTypes lastMovePlayer, Point lastMovePoint)
        {
            throw new NotImplementedException();
        }
    }
}
