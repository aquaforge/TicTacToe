namespace TicTacToeGame
{
    internal class PointEqualityComparer : IEqualityComparer<Point>
    {
        public bool Equals(Point? x, Point? y)
        {
            if (y == null && x == null) return true;
            if (x == null || y == null) return false;
            if (x.M == y.M && x.N == y.N) return true;
            return false;
        }

        public int GetHashCode(Point p) => (1000 + p.M + p.N).GetHashCode();
    }


}
