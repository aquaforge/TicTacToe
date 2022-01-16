using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public struct PlayerInfo
    {
        public PlayerTypes Player;
        public string AlgorithmName;
    }
}
