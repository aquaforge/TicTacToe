using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Point
    {
        public int M;
        public int N;

        public Point(int m, int n)
        {
            M = m;
            N = n;
        }

        public override string ToString() => $"({M},{N})";

        public Point Copy() => new (M, N);

    }
}
