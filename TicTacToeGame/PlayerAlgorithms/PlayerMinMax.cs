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
            return new Point(0, 0);
        }

        //        public static MovementSearchStatus FindBestMovementFromNow(PlayerTypes[,] board, out Point? newPoint,
        //            PlayerTypes forPlayer, PlayerTypes player,
        //            int level, double minMaxValue)
        //        {
        //            List<MovementSearch> movementSearch = GetEmptyPointsAround();
        //            foreach (var item in movementSearch)
        //            {
        //                item.MinMaxValue = minMaxValue;

        //                if (Game.VictoryСheck(item.Point, board))
        //                {
        //                    if (level == 1)
        //                    {
        //                        newPoint = new Point() { X = item.Point.X, Y = item.Point.Y };
        //                        return MovementSearchStatus.VICTORY;
        //                    }
        //                    item.MinMaxValue = minMaxValue + 5 * (SEARCH_DEPTH_AND_RANGE - level);
        //                }
        //                else
        //                {
        //                    PlayerTypes[,] b = Game.BoardCopy(board);
        //                    b[item.X, item.Y] = forPlayer;

        //                }
        //            }
        //        }



        //            //newPoint = new Point() { X = -100, Y = -100 };
        //            return MovementSearchStatus.DRAW;
        //        }




    }
}
