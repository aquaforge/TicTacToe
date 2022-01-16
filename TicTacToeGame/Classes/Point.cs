using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Point : IComparer<Point>
    {
        public int M;
        public int N;

        public Point(int m, int n)
        {
            M = m;
            N = n;
        }

        public override string ToString() => $"({M,2},{N,2})";

        public Point Copy() => new(M, N);

        public int Compare(Point? x, Point? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            if (x.M == y.M && x.N == y.N) return 0;
            return 1000 * (x.M + x.N) - (y.M + y.N);
        }

        static public Point Zero=> new Point(0,0);

    }


}
