using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class PlayerMinMax : IPlayerAI
    {
        public const int FITNESS_VICTORY_VALUE = 10000;
        const int SEARCH_CELL_RANGE = 2;
        const int SEARCH_DEPTH = 3;

        static Point[] directions = {
                new Point(1, 0), new Point(-1, 0),
                new Point(0, 1), new Point(0, -1),
                new Point(1, 1), new Point(1, -1),
                new Point(-1, 1), new Point(-1, -1)
            };

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;
        //cancelTokenSource.Cancel();

        readonly Random random = new Random();

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };


        public Point GetNextMovement(IGame g) => GetNextMovement(g.Board, g.PlayerToMove);
        public Point GetNextMovement(Board board, PlayerTypes playerToMove)
        {
            BoardToCheck data;
            token = cancelTokenSource.Token;

            data = new BoardToCheck(board.Copy(), isMyTurn: true, playerToMove, point: null, random);
            var root = new TreeNode<BoardToCheck>(data, null);
            CalculateFitness(root);

            double fitnessValue = root.Children.Max(d => d.Data.FitnessValue);
            BoardToCheck resultData = root.Children
                .Where(d => Math.Abs((double)d.Data.FitnessValue - (double)fitnessValue) < 0.01)
                .OrderBy(d => d.Data.ShuffleSort).Select(dd => dd.Data)
                .First();

            GeneralFileLogger.Log(Environment.NewLine + root.ToText());
            return resultData.LastAffectedCell.Copy();
        }


        private void CalculateFitness(TreeNode<BoardToCheck> node)
        {
            if (token.IsCancellationRequested) return;
            if (!node.IsRoot && node.Data.Point != null)
            {
                if (node.Data.Board.IsWon)
                {
                    node.Data.FitnessValue = (node.Data.IsMyTurn ? 1 : -1) 
                        * (FITNESS_VICTORY_VALUE - node.Level);
                    return;
                }
                else
                {
                    node.Data.FitnessValue = (node.Data.IsMyTurn ? 1 : -1)
                        * GetFintessValue(node.Data.Board, node.Data.Point.Row, node.Data.Point.Col, 3);
                }
            }

            if (node.Level >= SEARCH_DEPTH) return;

            PlayerTypes player = node.Data.Player;
            bool isMyTurn = node.IsRoot ? true : !node.Data.IsMyTurn;

            HashSet<Point> points = FindEmptyCells(node.Data.Board);
            foreach (Point p in points)
            {
                var board = node.Data.Board.Copy();
                board[p.Row, p.Col] = player;
                var data = new BoardToCheck(board, isMyTurn, Board.PlayerSwap(player), p.Copy(), random);
                node.AddChild(data);
            }



            ParallelLoopResult parallelResult = Parallel.ForEach(node.Children, new ParallelOptions { CancellationToken = token }, (child) =>
            { CalculateFitness(child); });
            Task.FromResult(parallelResult).Wait();

            //foreach (var child in node.Children)
            //    CalculateFitness(child);


            if (node.Children.Count == 0) return;

            if (isMyTurn)
                node.Data.FitnessValue = node.Children.Min(n => n.Data.FitnessValue);
            else
                node.Data.FitnessValue = node.Children.Max(n => n.Data.FitnessValue);
        }


        static HashSet<Point> FindEmptyCells(Board board)
        {
            HashSet<Point> points = new(new PointEqualityComparer());
            for (int row = 0; row < board.LengthRow; row++)
                for (int col = 0; col < board.LengthCol; col++)
                    if (board[row, col] != PlayerTypes.EMPTY)
                    {
                        for (int i = row - SEARCH_CELL_RANGE; i <= row + SEARCH_CELL_RANGE; i++)
                            for (int j = col - SEARCH_CELL_RANGE; j <= col + SEARCH_CELL_RANGE; j++)
                                if (board.IsInRange(i, j) && board[i, j] == PlayerTypes.EMPTY)
                                    points.Add(new Point(i, j));
                    }
            return points;
        }

        static public double GetFintessValue(Board board, int row, int col, int searchRange)
        {

            PlayerTypes player = board[row, col];
            if (player == PlayerTypes.EMPTY) return 0;

            if (searchRange < 1) searchRange = 1;

            double fitness = 0;
            for (int i = 1; i < searchRange; i++)
                foreach (Point direction in directions)
                {
                    Point p = new(row + i * direction.Row, col + i * direction.Col);
                    if (board.IsInRange(p))
                    {
                        if (board[p.Row, p.Col] == player)
                            fitness += 2f * (1 + searchRange - i) / searchRange;
                        else if (board[p.Row, p.Col] == PlayerTypes.EMPTY)
                            fitness += 1f * (1 + searchRange - i) / searchRange;
                    }
                }
            return fitness;
        }
    }
}
