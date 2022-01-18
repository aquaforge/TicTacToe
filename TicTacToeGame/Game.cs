using StateMachineLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame;

namespace TicTacToeGame
{
    [Serializable]
    public class Game : IStateMachineReady, IGame
    {

        private readonly Movements _movements = new();

        private Stopwatch? _stopWatch;

        private readonly StateMachine _stateMachine;

        private GameStates _gameState = GameStates.NEW_TURN_READY;
        public GameStates GameState => _gameState;
        public int State { get => (int)_gameState; }

        #region Board
        private readonly Board _board;
        public Board Board => _board.Copy();
        public int LengthRow { get => _board.LengthRow; }
        public int LengthCol { get => _board.LengthCol; }
        public PlayerTypes this[int row, int col] => _board[row, col];
        #endregion

        public int FilledCellsCount => _board.FilledCellsCount;
        public int EmptyCellsCount => _board.EmptyCellsCount;

        public PlayerTypes PlayerToMove => _movements.PlayerToMove;
        public PlayerTypes MovementLastPlayer => _movements.MovementLastPlayer;
        public Point MovementLastCell => _movements.MovementLastCell;
        public List<Movement> MovementsGetTop(int top = 0) => _movements.MovementsGetTop(top);
        public int MovementsCount() => _movements.Count;
        public bool IsGameFinished() => _gameState == GameStates.X_WON || _gameState == GameStates.O_WON || _gameState == GameStates.DRAW;




        public Game(byte row, byte col)
        {
            _board = new(row, col);

            _stateMachine = new(this);
            StateMachineInitialize();

            //Do predefined first move to the center of board
            _stateMachine.HandleEvent((int)GameEventes.START_THINKING, new object());
            _stateMachine.HandleEvent((int)GameEventes.DO_MOVE, new Point(LengthRow / 2, LengthCol / 2));
        }



        #region Game Actions for State Machine
        public void StateChange(int endState, Action<object>? action, object arguments)
        {
            _gameState = (GameStates)endState;
            GeneralFileLogger.Log($"STATE: {_gameState} {arguments}");
            action?.Invoke(arguments);
        }

        private void StateMachineInitialize()
        {
            #region Add Transition Rules
            _stateMachine.AddTransition((int)GameStates.NEW_TURN_READY, (int)GameEventes.START_THINKING, (int)GameStates.THINKING, ActionStartThinking);

            //PAUSED vs THINKING
            _stateMachine.AddTransition((int)GameStates.PAUSED, (int)GameEventes.START_THINKING, (int)GameStates.THINKING, ActionStartThinking);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.PAUSE, (int)GameStates.PAUSED, ActionPause);

            //DO_MOVE
            _stateMachine.AddTransition((int)GameStates.PAUSED, (int)GameEventes.DO_MOVE, (int)GameStates.CHECKING_RESULT, ActionMove);
            _stateMachine.AddTransition((int)GameStates.THINKING, (int)GameEventes.DO_MOVE, (int)GameStates.CHECKING_RESULT, ActionMove);

            //GAME_CHECK_FINISHED 
            _stateMachine.AddTransition((int)GameStates.CHECKING_RESULT, (int)GameEventes.X_WON, (int)GameStates.X_WON, null);
            _stateMachine.AddTransition((int)GameStates.CHECKING_RESULT, (int)GameEventes.O_WON, (int)GameStates.O_WON, null);
            _stateMachine.AddTransition((int)GameStates.CHECKING_RESULT, (int)GameEventes.DRAW, (int)GameStates.DRAW, null);
            _stateMachine.AddTransition((int)GameStates.CHECKING_RESULT, (int)GameEventes.TO_NEXT_TURN, (int)GameStates.NEW_TURN_READY, null);

            //UNDO_LAST_MOVE
            _stateMachine.AddTransition((int)GameStates.NEW_TURN_READY, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.NEW_TURN_READY, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.X_WON, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.NEW_TURN_READY, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.O_WON, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.NEW_TURN_READY, ActionUndoLastMove);
            _stateMachine.AddTransition((int)GameStates.DRAW, (int)GameEventes.UNDO_LAST_MOVE, (int)GameStates.NEW_TURN_READY, ActionUndoLastMove);
            #endregion
        }

        private void ActionStartThinking(object o)
        {
            if (_stopWatch == null) _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        private void ActionPause(object o)
        {
            _stopWatch?.Stop();
        }

        private void ActionUndoLastMove(object o)
        {
            if (_movements.Count == 0) return;
            Point point = MovementLastCell;
            _board[point.Row, point.Col] = PlayerTypes.EMPTY;
            _movements.RemoveLast();
        }

        private void ActionMove(object o)
        {
            if (o is not Point p) throw new ArgumentNullException("ActionMove");

            if (_board[p.Row, p.Col] != PlayerTypes.EMPTY) throw new ArgumentException($"{p} already filled in");

            PlayerTypes player = PlayerToMove;
            _board[p.Row, p.Col] = player;
            _movements.AddLast(new Movement(player: player, point: p, elapsedMilliseconds: _stopWatch?.ElapsedMilliseconds ?? 0));
            _stopWatch = null;

            if (_board.IsWon)
                _stateMachine.HandleEvent((int)(MovementLastPlayer == PlayerTypes.X ? GameEventes.X_WON : GameEventes.O_WON), new object());
            else if (_board.IsDraw)
                _stateMachine.HandleEvent((int)GameEventes.DRAW, new object());
            else
                _stateMachine.HandleEvent((int)GameEventes.TO_NEXT_TURN, new object());
        }
        #endregion

        #region Game Engine Public Methods
        public void DoMove(Point p) => _stateMachine.HandleEvent((int)GameEventes.DO_MOVE, p.Copy());
        public void DoPause() => _stateMachine.HandleEvent((int)GameEventes.PAUSE, new object());
        public void DoUndo() => _stateMachine.HandleEvent((int)GameEventes.UNDO_LAST_MOVE, new object());
        public void DoStartThinking() => _stateMachine.HandleEvent((int)GameEventes.START_THINKING, new object());
        #endregion

    }
}
