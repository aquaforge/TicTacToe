using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Movement
    {
        public PlayerTypes Player;
        public Point Point;
        public long ElapsedMilliseconds;

        public Movement(PlayerTypes player, Point point, long elapsedMilliseconds)
        {
            Player = player;
            Point = point;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public Movement Copy => new (Player, Point, ElapsedMilliseconds);

        public override string ToString() => $"{Player} {Point} {ElapsedMilliseconds}ms";
    }
}
