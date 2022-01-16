using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public interface IPlayerAI
    {
        public PlayerInfo GetPlayerInfo();
        public Point GetNextMovement(IGame g);

    }
}
