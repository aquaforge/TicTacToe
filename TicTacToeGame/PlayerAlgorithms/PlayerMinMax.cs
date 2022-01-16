//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TicTacToeGame
//{
//    public class PlayerMinMax : IPlayer
//    {
//        private const int SEARCH_DEPTH_AND_RANGE = 4;
//        public PlayerTypes Player { get; private set; }
//        public PlayerMinMax(PlayerTypes player)
//        {
//            if (player == PlayerTypes.EMPTY) throw new ArgumentException(player.ToString());
//            Player = player;
//        }

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

//    public MovementSearchStatus GetNextMovement(Game g, out Point? newPoint)
//    {
//        if (g.Player != Player) throw new ArgumentException($"Wrong player: {g.Player}");
//        if (g.GameStatus != GameStatuses.THINKING_X || g.GameStatus != GameStatuses.THINKING_O) throw new ArgumentException($"Wrong game status: {g.GameStatus}");
//        return FindBestMovementFromNow(g.Board, out newPoint, Player, 1);
//    }

//    public PlayerInfo GetPlayerInfo()
//    {
//        return new PlayerInfo() { Player = Player, AlgorithmName = nameof(PlayerMinMax) };
//    }
//}
//}
