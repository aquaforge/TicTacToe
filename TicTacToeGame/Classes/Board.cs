using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class Board
    {

        Point[] directions = {
                new Point(1, 0), new Point(-1, 0),
                new Point(0, 1), new Point(0, -1),
                new Point(1, 1), new Point(1, -1),
                new Point(-1, 1), new Point(-1, -1)
            };


        public const int FITNESS_VICTORY_VALUE = 1000;
        private const int SEARCH_CELL_RANGE = 4;
        private const int WIN_LENGTH = 5;

        private readonly PlayerTypes[,] _board;

        private readonly int _lengthRow;
        private readonly int _lengthCol;
        public int LengthRow { get => _lengthRow; }
        public int LengthCol { get => _lengthCol; }

        private Point? _lastAffectedCell;
        public Point? LastAffectedCell => _lastAffectedCell;

        private int _filledCellsCount = 0;
        public int FilledCellsCount { get => _filledCellsCount; }
        public int EmptyCellsCount { get => _lengthRow * _lengthCol - _filledCellsCount; }


        private bool _isWon = false;
        public bool IsWon => _isWon;
        public bool IsDraw => EmptyCellsCount == 0 && !IsWon;


        public Board(int m, int n)
        {
            _lengthRow = m;
            _lengthCol = n;
            _board = new PlayerTypes[_lengthRow, _lengthCol];
        }

        public Board Copy() => Board.Copy(this);

        public static Board Copy(Board b)
        {
            Board result = new(b.LengthRow, b.LengthCol);
            for (int m = 0; m < b.LengthRow; m++)
                for (int n = 0; n < b.LengthCol; n++)
                    result[m, n] = b[m, n];
            return result;

        }

        public PlayerTypes this[int m, int n]
        {
            get { return _board[m, n]; }
            set { SetCellValue(m, n, value); }
        }

        private void SetCellValue(int m, int n, PlayerTypes value)
        {
            if (_board[m, n] == value) return;

            switch (value)
            {
                case PlayerTypes.EMPTY:
                    _board[m, n] = value;
                    _filledCellsCount--;
                    _lastAffectedCell = null;
                    break;
                case PlayerTypes.X:
                case PlayerTypes.O:
                    if (_board[m, n] != PlayerTypes.EMPTY) throw new ArgumentException("X and O cannot be replaced");
                    _board[m, n] = value;
                    _filledCellsCount++;
                    _lastAffectedCell = new Point(m, n);
                    _isWon = IsVictory(m, n);
                    break;
                default:
                    break;
            }
        }


        public double CalculateFintess(int m, int n, int searchRange)
        {

            PlayerTypes player = _board[m, n];
            if (player == PlayerTypes.EMPTY) return 0;

            if (searchRange < 1) searchRange = 1;

            double fitness = 0;
            for (int i = 1; i < searchRange; i++)
                foreach (Point direction in directions)
                {
                    Point p = new(m + i * direction.Row, n + i * direction.Col);
                    if (IsInRange(p))
                    {
                        if (_board[p.Row, p.Col] == player)
                            fitness += 2f * (1 + searchRange - i) / searchRange;
                        else if (_board[p.Row, p.Col] == PlayerTypes.EMPTY)
                            fitness += 1f * (1 + searchRange - i) / searchRange;
                    }
                }
            return fitness;
        }

        public bool IsVictory(int m, int n)
        {
            int maxLength;

            PlayerTypes player = _board[m, n];
            if (player == PlayerTypes.EMPTY) return false;

            //horisontal
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthRow && _board[m + i, n] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && _board[m - i, n] == player) maxLength++; else break;
            if (maxLength >= 5) 
                return true;

            //vertical
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (n + i < _lengthCol && _board[m, n + i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (n - i >= 0 && _board[m, n - i] == player) maxLength++; else break;
            if (maxLength >= 5) 
                return true;

            //diagonal \
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthRow && n + i < _lengthCol && _board[m + i, n + i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && n - i >= 0 && _board[m - i, n - i] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            //diagonal /
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthRow && n - i >= 0 && _board[m + i, n - i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && n + i < _lengthCol && _board[m - i, n + i] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            return false;


        }

        public bool IsInRange(int m, int n) => m >= 0 && n >= 0 && m < _lengthRow && n < _lengthCol;
        public bool IsInRange(Point p) => IsInRange(p.Row, p.Col);


    }
}

