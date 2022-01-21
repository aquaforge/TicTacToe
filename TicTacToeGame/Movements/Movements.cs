using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Movements
    {
        private readonly LinkedList<Movement> _movements = new();

        public PlayerTypes MovementLastPlayer { get; private set; } = PlayerTypes.EMPTY;
        public Point MovementLastCell { get; private set; } = Point.MaxPoint;
        public PlayerTypes PlayerToMove => MovementLastPlayer == PlayerTypes.X ? PlayerTypes.O : PlayerTypes.X;
        public int Count => _movements.Count;

        private void ApplyPublicProps()
        {
            Movement m = _movements.Last();

            MovementLastPlayer = m.Player;
            MovementLastCell = m.Point.Copy();
        }

        public void AddLast(Movement m)
        {
            _movements.AddLast(m);
            ApplyPublicProps();
        }

        public void RemoveLast()
        {
            _movements.RemoveLast();
            ApplyPublicProps();
        }

        public List<Movement> MovementsGetTop(int top = 0)
        {
            List<Movement> result = _movements.ToList();
            if (top == 0 || result.Count <= top) return result;
            return result.GetRange(result.Count - top, top);
        }

    }
}
