using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    internal class MinMaxTreeNodeData
    {
        public Point Point;
        public bool IsMyTurn;
        public double MinMaxValue;

        public MinMaxTreeNodeData(Point point, bool isMyTurn, double minMaxValue = 0)
        {
            Point = point;
            IsMyTurn = isMyTurn;
            MinMaxValue = minMaxValue;
        }
    }
}
