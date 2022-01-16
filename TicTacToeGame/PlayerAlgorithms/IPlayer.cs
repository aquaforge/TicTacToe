using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public interface IPlayer
    {
        public PlayerInfo GetPlayerInfo();
        public MovementSearchStatus GetNextMovement(Game g, out Point? newPoint);

    }
}
