using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class Board
    {



        public const int FITNESS_VICTORY_VALUE = 10000;
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
                    result._board[m, n] = b[m, n];

            result._isWon = b.IsWon;
            result._lastAffectedCell = b.LastAffectedCell?.Copy();
            result._filledCellsCount = b.FilledCellsCount;

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




        public bool IsVictory(int m, int n)
        {
            int maxLength;

            PlayerTypes player = _board[m, n];
            if (player == PlayerTypes.EMPTY) return false;

            //horisontal
            maxLength = 1;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m + i < _lengthRow && _board[m + i, n] == player) maxLength++; else break;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m - i >= 0 && _board[m - i, n] == player) maxLength++; else break;
            if (maxLength >= WIN_LENGTH) 
                return true;

            //vertical
            maxLength = 1;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (n + i < _lengthCol && _board[m, n + i] == player) maxLength++; else break;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (n - i >= 0 && _board[m, n - i] == player) maxLength++; else break;
            if (maxLength >= WIN_LENGTH) 
                return true;

            //diagonal \
            maxLength = 1;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m + i < _lengthRow && n + i < _lengthCol && _board[m + i, n + i] == player) maxLength++; else break;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m - i >= 0 && n - i >= 0 && _board[m - i, n - i] == player) maxLength++; else break;
            if (maxLength >= WIN_LENGTH) return true;

            //diagonal /
            maxLength = 1;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m + i < _lengthRow && n - i >= 0 && _board[m + i, n - i] == player) maxLength++; else break;
            for (int i = 1; i < WIN_LENGTH; i++)
                if (m - i >= 0 && n + i < _lengthCol && _board[m - i, n + i] == player) maxLength++; else break;
            if (maxLength >= WIN_LENGTH) return true;

            return false;


        }

        public bool IsInRange(int m, int n) => m >= 0 && n >= 0 && m < _lengthRow && n < _lengthCol;
        public bool IsInRange(Point p) => IsInRange(p.Row, p.Col);


    }
}

