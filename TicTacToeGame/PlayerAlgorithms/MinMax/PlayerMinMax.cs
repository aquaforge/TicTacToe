using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class PlayerMinMax : IPlayerAI
    {
        private const int SEARCH_DEPTH_AND_RANGE = 4;

        public PlayerInfo GetPlayerInfo() => new() { AlgorithmName = nameof(PlayerMinMax) };

        public Point GetNextMovement(IGame g)
        {
            TreeNode<MinMaxTreeNodeData> root = new(new MinMaxTreeNodeData(Point.Zero, false));

            CheckLevel(g.Board, root, true);

            MinMaxTreeNodeData? resultData = root.Children.OrderByDescending(d => d.Data.MinMaxValue).Select(dd => dd.Data).FirstOrDefault();
            if (resultData == null) throw new Exception("resultData is  null");
            return resultData.Point.Copy();
        }

        private void CheckLevel(PlayerTypes[,] board, TreeNode<MinMaxTreeNodeData> root, bool isMyTurn)
        {
            int lengthM = board.GetLength(0);
            int lengthN = board.GetLength(1);

            HashSet<Point> points = new(new PointEqualityComparer());
            for (int m = 0; m < lengthM; m++)
                for (int n = 0; n < lengthN; n++)
                    if (board[m, n] != PlayerTypes.EMPTY)
                    {
                        for (int i = m - SEARCH_DEPTH_AND_RANGE; i <= m + SEARCH_DEPTH_AND_RANGE; i++)
                            for (int j = n - SEARCH_DEPTH_AND_RANGE; j <= n + SEARCH_DEPTH_AND_RANGE; j++)
                                if (i >= 0 && j >= 0 && i < lengthN && j < lengthM && board[i, j] == PlayerTypes.EMPTY) points.Add(new Point(i, j));
                    }

            foreach (Point p in points)
                root.AddChild(new MinMaxTreeNodeData(p.Copy(), isMyTurn));

            //TODO
        }

    }
}
