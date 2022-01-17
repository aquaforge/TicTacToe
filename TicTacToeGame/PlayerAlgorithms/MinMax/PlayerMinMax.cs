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

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };

        public Point GetNextMovement(IGame g)
        {
            TreeNode<MinMaxTreeNodeData> root = new(new MinMaxTreeNodeData(g.PlayerToMove, Point.MaxPoint, true, g.Board), null);
            CalculateFitness(root);

            MinMaxTreeNodeData? resultData = root.Children.OrderByDescending(d => d.Data?.FitnessValue).Select(dd => dd.Data).FirstOrDefault();
            if (resultData == null) throw new Exception("resultData is  null");
            return resultData.Point.Copy();
        }

        private void CalculateFitness(TreeNode<MinMaxTreeNodeData> node)
        {
            MinMaxTreeNodeData data = node.Data;
            HashSet<Point> points = new(new PointEqualityComparer());

            Board board = data.Board.Copy();
            PlayerTypes player = data.Player;
            if (!data.IsMyTurn) player = (player == PlayerTypes.O ? PlayerTypes.X : PlayerTypes.O);
            if (!node.IsRoot) board[data.Point.M, data.Point.N] = player;



            if (node.Level < SEARCH_DEPTH)
            {
                for (int m = 0; m < board.LengthM; m++)
                    for (int n = 0; n < board.LengthN; n++)
                        if (board[m, n] != PlayerTypes.EMPTY)
                        {
                            for (int i = m - SEARCH_CELL_RANGE; i <= m + SEARCH_CELL_RANGE; i++)
                                for (int j = n - SEARCH_CELL_RANGE; j <= n + SEARCH_CELL_RANGE; j++)
                                    if (i >= 0 && j >= 0 && i < board.LengthN && j < board.LengthM && board[i, j] == PlayerTypes.EMPTY)
                                        points.Add(new Point(i, j));
                        }
            }

            if (points.Count > 0)
            {
                Parallel.ForEach(points, (p) =>
                {
                    TreeNode<MinMaxTreeNodeData> child = node.AddChild(new MinMaxTreeNodeData(data.Player, p.Copy(), !data.IsMyTurn, board));
                    CalculateFitness(child);
                });
                double fitness = node.Children.Max(c => c.Data.FitnessValue);
                data.FitnessValue = fitness;
            }
            else
            {
                if (!node.IsRoot)
                {
                    double fitness = board.CalculateFintess(data.Point.M, data.Point.N);
                    //store fitness with Tree level corrections and Enemy coefficient
                    data.FitnessValue = fitness * (data.IsMyTurn ? 1 : -1) * (100f - node.Level) / 100f;
                }
            }


        }

    }
}
