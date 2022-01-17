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

        public MinMaxTreeNodeData(PlayerTypes player, Point point, bool isMyTurn, Board board, double fitnessValue = 0)
        {
            Player = player;
            Point = point;
            IsMyTurn = isMyTurn;
            FitnessValue = fitnessValue;
            Board = board;
        }

    }
}
