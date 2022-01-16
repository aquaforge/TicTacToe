using StateMachineLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Game : IStateMachineReady
    {

        private readonly LinkedList<Movement> _movements = new();
        private readonly Stopwatch stopWatch = new();

        private readonly StateMachine _stateMachine;

        private readonly PlayerTypes[,] _board;
        private readonly int _lengthM;
        private readonly int _lengthN;

        private GameStates _gameState = GameStates.PAUSED;
        public GameStates GameState => _gameState;
        public int State { get => (int)_gameState; }
        public int LengthM { get => _lengthM; }
        public int LengthN { get => _lengthN; }

        public PlayerTypes PlayerToMove
        {
            get
            {
                if (_movements.Count == 0) return PlayerTypes.X;
                return LastMovePlayer == PlayerTypes.O ? PlayerTypes.X : PlayerTypes.O;
            }
        }

        public PlayerTypes[,] Board
        {
            get { return Matrixes.MatrixCopy<PlayerTypes>(_board); }
        }

        public LinkedList<Movement> Movements
        {
            get { return MovementsCopy(_movements); }
        }

        public PlayerTypes this[int m, int n]
        {
            get { return _board[m, n]; }
        }

        public PlayerTypes LastMovePlayer => _movements.Last().Player;
        public Point LastMovePoint => _movements.Last().Point.Copy();


        #region Game Actions

        private void ActionStartThinking(object o)
        {
            _gameState = GameStates.THINKING;
            stopWatch.Start();
        }

        private void ActionPause(object o)
        {
            _gameState = GameStates.PAUSED;
            stopWatch.Stop();
        }

        private void ActionUndoLastMove(object o)
        {
            if (_movements.Count == 0) return;
            Point point = LastMovePoint;
            _board[point.M, point.N] = PlayerTypes.EMPTY;
            _movements.RemoveLast();
        }

        private void ActionCheckVictory(object o)
        {
            if (Matrixes.IsWon(Board, LastMovePlayer, LastMovePoint))
            {
                _stateMachine.HandleEvent((int)(LastMovePlayer == PlayerTypes.X ? GameEventes.X_WON : GameEventes.O_WON), new object());
                return;
            }

            if (_movements.Count == LengthM * LengthN) // все ячейки заполнены
            {
                _stateMachine.HandleEvent((int)GameEventes.DRAW, new object());
                return;
            }
        }

        private void ActionMove(object o)
        {
            if (o is not Point p) throw new ArgumentNullException("ActionMove");

            if (_board[p.M, p.N] != PlayerTypes.EMPTY) throw new ArgumentException($"{p} already used");

            PlayerTypes player = PlayerToMove;
            _board[p.M, p.N] = player;
            _movements.AddLast(new Movement(player: player, point: p, elapsedMilliseconds: stopWatch.ElapsedMilliseconds));
            stopWatch.Restart();
        }
        #endregion


        #region public Methods

        public void DoMove(Point p) => _stateMachine.HandleEvent((int)GameEventes.DO_MOVE, p.Copy());
        public void DoPause() => _stateMachine.HandleEvent((int)GameEventes.PAUSE, new object());
        public void DoUndo() => _stateMachine.HandleEvent((int)GameEventes.UNDO_LAST_MOVE, new object());
        public void DoStartThinking() => _stateMachine.HandleEvent((int)GameEventes.START_THINKING, new object());

        #endregion


        private void StateMachineInitialize()
        {

            #region Add Transition Rules
            //PAUSED
            _stateMachine.AddTransition((int)GameStates.PAUSED, (int)GameEventes.START_THINKING, (int)GameStates.THINKING, ActionStartThinking);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.PAUSE, (int)GameStates.PAUSED, ActionPause);

            //DO_MOVE
            _stateMachine.AddTransition((int)GameStates.PAUSED, (int)GameEventes.DO_MOVE, (int)GameStates.THINKING, ActionMove);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.DO_MOVE, (int)GameStates.THINKING, ActionMove);

            //GAME_FINISHED 
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.X_WON, (int)GameStates.X_WON, null);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.O_WON, (int)GameStates.O_WON, null);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.DRAW, (int)GameStates.DRAW, null);

            //UNDO_LAST_MOVE
            _stateMachine.AddTransition((int)GameStates.PAUSED, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.PAUSED, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.PAUSED, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.X_WON, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.PAUSED, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.O_WON, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.PAUSED, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.DRAW, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.PAUSED, ActionUndoLastMove);
            #endregion
        }

        public Game(int m, int n)
        {
            _lengthM = m;
            _lengthN = n;

            _board = new PlayerTypes[LengthM, LengthN];

            _stateMachine = new(this);
            StateMachineInitialize();

            //Do predefined first move to the center of board
            _gameState = GameStates.PAUSED;
            _stateMachine.HandleEvent((int)GameEventes.DO_MOVE, new Point(LengthM / 2, LengthN / 2));
        }



        public static LinkedList<Movement> MovementsCopy(LinkedList<Movement> movement)
        {
            var q = new LinkedList<Movement>();
            for (int i = 0; i < movement.Count - 1; i++)
            {
                Movement m = movement.ElementAt(i);
                q.AddLast(new Movement(player: m.Player, elapsedMilliseconds: m.ElapsedMilliseconds, point: new Point(m.Point.M, m.Point.N)));
            }
            return q;
        }

        public void StateChange(int endState, Action<object>? action, object arguments)
        //public void StateChange(int endState, Action<GameActionArgument>? action, GameActionArgument arguments)
        {
            _gameState = (GameStates)endState;
            action?.Invoke(arguments);
        }



    }
}
