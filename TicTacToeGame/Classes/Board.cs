using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class Board
    {
        const int FITNESS_VICTORY_VALUE = 1000;
        private const int SEARCH_CELL_RANGE = 4;

        private readonly PlayerTypes[,] _board;

        private readonly int _lengthM;
        private readonly int _lengthN;
        public int LengthM { get => _lengthM; }
        public int LengthN { get => _lengthN; }

        private Point? _lastAffectedCell;
        public Point? LastAffectedCell => _lastAffectedCell;

        private int _filledCellsCount = 0;
        public int FilledCellsCount { get => _filledCellsCount; }
        public int EmptyCellsCount { get => _lengthM * _lengthN - _filledCellsCount; }


        private bool _isWon = false;
        public bool IsWon => _isWon;
        public bool IsDraw => EmptyCellsCount == 0 && !IsWon;


        public Board(int m, int n)
        {
            _lengthM = m;
            _lengthN = n;
            _board = new PlayerTypes[LengthM, LengthN];
        }

        public Board Copy() => Board.Copy(this);

        public static Board Copy(Board b)
        {
            Board result = new(b.LengthM, b.LengthN);
            for (int m = 0; m < b.LengthM; m++)
                for (int n = 0; n < b.LengthN; n++)
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

        internal double CalculateFintess(int m, int n)
        {

            PlayerTypes player = _board[m, n];
            if (player == PlayerTypes.EMPTY) return 0;

            if (IsVictory(m, n)) return FITNESS_VICTORY_VALUE;

            int count = 0;
            for (int i = m - SEARCH_CELL_RANGE; i <= m + SEARCH_CELL_RANGE; i++)
                for (int j = n - SEARCH_CELL_RANGE; j <= n + SEARCH_CELL_RANGE; j++)
                    if (i >= 0 && j >= 0 && i < _lengthM && j < _lengthN && _board[i, j] == player)
                        count++;

            //Fitness are IsVictory + number of filled by player cells nearby
            return count;
        }

        public bool IsVictory(int m, int n)
        {
            int maxLength;

            PlayerTypes player = _board[m, n];
            if (player == PlayerTypes.EMPTY) return false;

            //horisontal
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthM && _board[m + i, n] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && _board[m - i, n] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            //vertical
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (n + i < _lengthN && _board[m, n + i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (n - i >= 0 && _board[m, n - i] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            //diagonal \
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthM && n + i < _lengthN && _board[m + i, n + i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && n - i >= 0 && _board[m - i, n - i] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            //diagonal /
            maxLength = 1;
            for (int i = 1; i < 5; i++)
                if (m + i < _lengthM && n - i >= 0 && _board[m + i, n - i] == player) maxLength++; else break;
            for (int i = 1; i < 5; i++)
                if (m - i >= 0 && n + i >= _lengthN && _board[m - i, n + i] == player) maxLength++; else break;
            if (maxLength >= 5) return true;

            return false;
        }

    }
}

