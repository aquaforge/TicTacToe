using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    internal class BoardToCheck
    {
        public PlayerTypes Player;
        public bool IsMyTurn;
        public Board Board;
        public Point? Point;
        public double FitnessValue = 0;
        public byte ShuffleSort { get; init; }

        public Point LastAffectedCell => Board?.LastAffectedCell ?? Point.MaxPoint;

        public BoardToCheck(Board board, bool isMyTurn, PlayerTypes playerToMove, Point? point, Random? random = null)
        {
            Board = board.Copy();
            IsMyTurn = isMyTurn;
            Player = playerToMove;
            Point = point;
            if (random != null) ShuffleSort = (byte)random.Next(255);
        }

        public override string ToString() => $"{Player} MyTurn={IsMyTurn} {LastAffectedCell} {FitnessValue:0.##}";

    }
}
