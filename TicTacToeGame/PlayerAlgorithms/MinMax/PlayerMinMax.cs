using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class PlayerMinMax : IPlayerAI
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token;


        private const int SEARCH_CELL_RANGE = 2;
        private const int SEARCH_DEPTH = 3;

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };

        public Point GetNextMovement(IGame g)
        {
            token = cancelTokenSource.Token;
            //cancelTokenSource.Cancel();
            //if (token.IsCancellationRequested) {}


            TreeNode<MinMaxTreeNodeData> root = new(new MinMaxTreeNodeData(g.PlayerToMove, Point.MaxPoint, true, g.Board), null);
            CalculateFitness(root);


            string s = Environment.NewLine;
            foreach (var item in root.Children.OrderByDescending(d => d.Data?.FitnessValue).Select(dd => dd.Data))
            {
                s += item.ToString() + Environment.NewLine;
            }
            GeneralFileLogger.Log(s);
            //GeneralFileLogger.Log(new Tree<MinMaxTreeNodeData>(root).ToString());

            MinMaxTreeNodeData? resultData = root.Children.OrderByDescending(d => d.Data?.FitnessValue).Select(dd => dd.Data).FirstOrDefault();
            if (resultData == null) throw new Exception("resultData is  null");
            return resultData.Point.Copy();
        }

        private void CalculateFitness(TreeNode<MinMaxTreeNodeData> node)
        {
            double fitness = 0;

            if (token.IsCancellationRequested) return;

            MinMaxTreeNodeData data = node.Data;

            Board board = data.Board.Copy();
            PlayerTypes player = data.Player;
            if (!data.IsMyTurn) player = (player == PlayerTypes.O ? PlayerTypes.X : PlayerTypes.O);

            if (!node.IsRoot)
            {
                board[data.Point.M, data.Point.N] = player;
                if (board.IsWon)
                    fitness = Board.FITNESS_VICTORY_VALUE;
                else
                    fitness = board.CalculateFintess(data.Point.M, data.Point.N, SEARCH_CELL_RANGE);
                data.FitnessValue = fitness * (data.IsMyTurn ? 1 : -1) * (100f - node.Level) / 100f;
                if (board.IsWon) return;
            }

            if (node.Level < SEARCH_DEPTH)
            {
                HashSet<Point> points = new(new PointEqualityComparer());
                for (int m = 0; m < board.LengthM; m++)
                    for (int n = 0; n < board.LengthN; n++)
                        if (board[m, n] != PlayerTypes.EMPTY)
                        {
                            for (int i = m - SEARCH_CELL_RANGE; i <= m + SEARCH_CELL_RANGE; i++)
                                for (int j = n - SEARCH_CELL_RANGE; j <= n + SEARCH_CELL_RANGE; j++)
                                    if (i >= 0 && j >= 0 && i < board.LengthM && j < board.LengthN && board[i, j] == PlayerTypes.EMPTY)
                                        points.Add(new Point(i, j));
                        }
                foreach (Point p in points)
                    node.AddChild(new MinMaxTreeNodeData(data.Player, p.Copy(), !data.IsMyTurn, board));
            }
            if (node.Children.Count == 0) return;

            //ParallelLoopResult parallelResult = Parallel.ForEach(points, new ParallelOptions { CancellationToken = token }, (p) =>
            foreach (var child in node.Children)
            {
                CalculateFitness(child);
            }
            //); Task.FromResult(parallelResult).Wait();

            fitness = node.Children.Max(c => c.Data.FitnessValue);
            data.FitnessValue += fitness;
        }

    }
}
