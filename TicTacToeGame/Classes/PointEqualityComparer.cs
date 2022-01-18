namespace TicTacToeGame
{
    internal class PointEqualityComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point? x, Point? y)
        {
            if (y == null && x == null) return true;
            if (x == null || y == null) return false;
            if (x.Row == y.Row && x.Col == y.Col) return true;
            return false;
        }

        public int GetHashCode(Point p) => (1000 + p.Row + p.Col).GetHashCode();
    }


}
