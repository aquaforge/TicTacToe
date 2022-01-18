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


        public Board(int row, int col)
        {
            _lengthRow = row;
            _lengthCol = col;
            _board = new PlayerTypes[_lengthRow, _lengthCol];
        }

        public Board Copy() => Board.Copy(this);

        public static Board CreateExactPosition(string[] matrix, Point lastAffectedCell)
        {
            Board board = new(matrix.Length, matrix[0].Length);

            for (int i = 1; i < board.LengthRow; i++)
                if (matrix[0].Length != matrix[i].Length) throw new ArgumentException($"Wrong matrix dimention [_,{i}]:{matrix[i].Length}");

            for (int row = 0; row < board.LengthRow; row++)
            {
                char[] chars = matrix[row].ToCharArray();
                for (int col = 0; col < board.LengthCol; col++)
                {
                    if (chars[col] == ' ')
                        board[row, col] = PlayerTypes.EMPTY;
                    else
                        board[row, col] = (PlayerTypes)Enum.Parse(typeof(PlayerTypes), chars[col].ToString(), true);
                }
            }

            int _filledCellsCount = 0;
            for (int row = 1; row < board._lengthRow; row++)
                for (int col = 1; col < board._lengthCol; col++)
                    if (board._board[row, col] != PlayerTypes.EMPTY) _filledCellsCount++;

            board._lastAffectedCell = lastAffectedCell;
            board._isWon = board.IsVictory(lastAffectedCell.Row, lastAffectedCell.Col);

            return board;
        }


        public static Board Copy(Board b)
        {
            Board result = new(b.LengthRow, b.LengthCol);

            for (int row = 0; row < b.LengthRow; row++)
                for (int col = 0; col < b.LengthCol; col++)
                    result._board[row, col] = b[row, col];

            result._isWon = b.IsWon;
            result._lastAffectedCell = b.LastAffectedCell?.Copy();
            result._filledCellsCount = b.FilledCellsCount;

            return result;

        }

        public PlayerTypes this[int row, int col]
        {
            get { return _board[row, col]; }
            set { SetCellValue(row, col, value); }
        }




        private void SetCellValue(int row, int col, PlayerTypes value)
        {
            if (_board[row, col] == value) return;

            switch (value)
            {
                case PlayerTypes.EMPTY:
                    _board[row, col] = value;
                    _filledCellsCount--;
                    _lastAffectedCell = null;
                    break;
                case PlayerTypes.X:
                case PlayerTypes.O:
                    if (_board[row, col] != PlayerTypes.EMPTY) throw new ArgumentException("X and O cannot be replaced");
                    _board[row, col] = value;
                    _filledCellsCount++;
                    _lastAffectedCell = new Point(row, col);
                    _isWon = IsVictory(row, col);
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


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"WON={IsWon} DRAW={IsDraw}");
            if (_lastAffectedCell != null) sb.Append($" LAST_MOVE={_lastAffectedCell}");

            for (int row = 0; row < _lengthRow; row++)
            {
                sb.Append('|');
                for (int col = 0; col < _lengthCol; col++)
                    sb.Append(_board[row, col].ToString());
            }

            return sb.ToString();
        }

    }
}

