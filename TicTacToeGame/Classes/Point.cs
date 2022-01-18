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
        public int Row;
        public int Col;

        public Point(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"[{Row,2},{Col,2}]";

        public Point Copy() => new(Row, Col);

        public int Compare(Point? row, Point? col)
        {
            if (row == null && col == null) return 0;
            if (row == null) return -1;
            if (col == null) return 1;
            if (row.Row == col.Row && row.Col == col.Col) return 0;
            return 1000 * (row.Row + row.Col) - (col.Row + col.Col);
        }

        static public Point Zero => new(0, 0);
        static public Point MaxPoint => new(int.MaxValue, int.MaxValue);

    }


}
