using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    internal class MinMaxTreeNodeData
    {
        public PlayerTypes Player;
        public Point Point;
        public bool IsMyTurn;
        public Board Board;
        public byte ShuffleSort { get; private set; }


        private bool _isFitnessCalculated = false;
        public bool IsFitnessCalculated => _isFitnessCalculated;

        private double _fitnessValue = 0;
        public double FitnessValue
        {
            get => _fitnessValue;
            set
            {
                _fitnessValue = value;
                _isFitnessCalculated = true;
            }
        }

        public MinMaxTreeNodeData(PlayerTypes player, Point point, bool isMyTurn, Board board, Random? random = null, double fitnessValue = 0)
        {
            Player = player;
            Point = point;
            IsMyTurn = isMyTurn;
            FitnessValue = fitnessValue;
            Board = board;

            if (random != null) ShuffleSort = (byte)random.Next(255);
        }

        public override string ToString() => $"{Player} {Point} {FitnessValue:0.##}";

    }
}
