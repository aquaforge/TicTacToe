namespace TicTacToeGame
{
    public interface IGame
    {
        PlayerTypes this[int m, int n] { get; }

        Board Board { get; }
        int EmptyCellsCount { get; }
        int FilledCellsCount { get; }
        GameStates GameState { get; }
        Point MovementLastCell { get; }
        PlayerTypes MovementLastPlayer { get; }
        int LengthM { get; }
        int LengthN { get; }
        PlayerTypes PlayerToMove { get; }

        void DoMove(Point p);
        void DoPause();
        void DoStartThinking();
        void DoUndo();

        public List<Movement> MovementsGetTop(int top);
        public int MovementsCount();
    }
}