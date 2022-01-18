using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class PlayerMinMax : IPlayerAI
    {

        private const int SEARCH_CELL_RANGE = 2;
        private const int SEARCH_DEPTH = 3;

        private readonly Point[] directions = {
                new Point(1, 0), new Point(-1, 0),
                new Point(0, 1), new Point(0, -1),
                new Point(1, 1), new Point(1, -1),
                new Point(-1, 1), new Point(-1, -1)
            };

        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;

        Random random = new Random();

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };

        public Point GetNextMovement(IGame g)
        {
            token = cancelTokenSource.Token;
            //cancelTokenSource.Cancel();
            //if (token.IsCancellationRequested) {}


            TreeNode<MinMaxTreeNodeData> root = new(new MinMaxTreeNodeData(g.PlayerToMove, Point.MaxPoint, false, g.Board), null);
            CalculateFitness(root);


            string s = Environment.NewLine;
            foreach (var item in root.Children.OrderByDescending(d => d.Data?.FitnessValue).Select(dd => dd.Data))
            {
                s += item.ToString() + Environment.NewLine;
            }
            GeneralFileLogger.Log(s);

            double fitnessValue = root.Children.Max(d => d.Data.FitnessValue);
            MinMaxTreeNodeData resultData = root.Children
                .Where(d => Math.Abs((double)d.Data.FitnessValue - (double)fitnessValue) < 0.001)
                .OrderBy(d => d.Data.ShuffleSort).Select(dd => dd.Data)
                .First();
            return resultData.Point.Copy();
        }


        public double CalculateFintess(Board board, int row, int col, int searchRange)
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

        private void CalculateFitness(TreeNode<MinMaxTreeNodeData> node)
        {
            if (token.IsCancellationRequested) return;

            MinMaxTreeNodeData data = node.Data;
            Board board = data.Board;

            PlayerTypes player = data.Player;
            if (!data.IsMyTurn) player = (player == PlayerTypes.O ? PlayerTypes.X : PlayerTypes.O);

            if (!node.IsRoot)
            {
                board[data.Point.Row, data.Point.Col] = player;
                if (board.IsWon)
                {
                    data.FitnessValue = (data.IsMyTurn ? 1 : -1) * (Board.FITNESS_VICTORY_VALUE - node.Level);
                    GeneralFileLogger.Log(node.Level + " isWon " + data.ToString());
                    return;
                }
            }

            if (node.Level < SEARCH_DEPTH)
            {
                HashSet<Point> points = new(new PointEqualityComparer());
                for (int row = 0; row < board.LengthRow; row++)
                    for (int col = 0; col < board.LengthCol; col++)
                        if (board[row, col] != PlayerTypes.EMPTY)
                        {
                            for (int i = row - SEARCH_CELL_RANGE; i <= row + SEARCH_CELL_RANGE; i++)
                                for (int j = col - SEARCH_CELL_RANGE; j <= col + SEARCH_CELL_RANGE; j++)
                                    if (i >= 0 && j >= 0 && i < board.LengthRow && j < board.LengthCol && board[i, j] == PlayerTypes.EMPTY)
                                        points.Add(new Point(i, j));
                        }
                foreach (Point point in points)
                    node.AddChild(new MinMaxTreeNodeData(data.Player, point.Copy(), !data.IsMyTurn, board.Copy(), random));
            }
            if (node.Children.Count == 0) return;

            //ParallelLoopResult parallelResult = Parallel.ForEach(node.Children, new ParallelOptions { CancellationToken = token }, (child) =>
            foreach (var child in node.Children)
            {
                CalculateFitness(child);
            }
            //); Task.FromResult(parallelResult).Wait();

            if (data.IsMyTurn)
                data.FitnessValue = node.Children.Max(n => n.Data.FitnessValue);
            else
                data.FitnessValue = node.Children.Min(n => n.Data.FitnessValue);

            if (data.FitnessValue != 0) GeneralFileLogger.Log(node.Level + " " + data.ToString());

            //fitness = node.Children.Max(c => c.Data.FitnessValue);
            //data.FitnessValue += fitness;

            //    else
            //    fitness = CalculateFintess(board, data.Point.Row, data.Point.Col, SEARCH_CELL_RANGE);
            //data.FitnessValue = fitness * (100f - node.Level) / 100f * (data.IsMyTurn ? 1 : -1);
            //if (board.IsWon) return;


        }

    }
}
