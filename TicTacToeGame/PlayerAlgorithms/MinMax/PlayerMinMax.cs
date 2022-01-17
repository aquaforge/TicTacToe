using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class PlayerMinMax : IPlayerAI
    {
        private const int SEARCH_CELL_RANGE = 4;
        private const int SEARCH_DEPTH = 5;

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };

        public Point GetNextMovement(IGame g)
        {
            TreeNode<MinMaxTreeNodeData> root = new(new MinMaxTreeNodeData(g.PlayerToMove, Point.MaxPoint, true, g.Board), null);
            CalculateFitness(root);

            MinMaxTreeNodeData? resultData = root.Children.OrderByDescending(d => d.Data?.FitnessValue).Select(dd => dd.Data).FirstOrDefault();
            if (resultData == null) throw new Exception("resultData is  null");
            return resultData.Point.Copy();
        }

        private void CalculateFitness(TreeNode<MinMaxTreeNodeData> root)
        {
            MinMaxTreeNodeData data = root.Data;
            HashSet<Point> points = new(new PointEqualityComparer());

            if (root.Level <= SEARCH_DEPTH)
            {
                for (int m = 0; m < data.Board.LengthM; m++)
                    for (int n = 0; n < data.Board.LengthN; n++)
                        if (data.Board[m, n] != PlayerTypes.EMPTY)
                        {
                            for (int i = m - SEARCH_CELL_RANGE; i <= m + SEARCH_CELL_RANGE; i++)
                                for (int j = n - SEARCH_CELL_RANGE; j <= n + SEARCH_CELL_RANGE; j++)
                                    if (i >= 0 && j >= 0 && i < data.Board.LengthN && j < data.Board.LengthM && data.Board[i, j] == PlayerTypes.EMPTY)
                                        points.Add(new Point(i, j));
                        }
            }

            if (points.Count > 0)
            {
                Board b = data.Board.Copy();
                b[data.Point.M, data.Point.N] = data.Player;
                foreach (Point p in points)
                {
                    TreeNode<MinMaxTreeNodeData> child = root.AddChild(new MinMaxTreeNodeData(data.Player, p.Copy(), !data.IsMyTurn, b));
                    CalculateFitness(child);
                }
            }
            else
            {
                data.FitnessValue = data.Board.CalculateFintess(data.Point.M, data.Point.N);
            }

            //TODO
        }

    }
}
