namespace TicTacToeGame
{
    [Serializable]
    public enum GameEventes
    {
        PAUSE,
        START_THINKING,
        DO_MOVE,
        TO_NEXT_TURN,
        X_WON,
        O_WON,
        DRAW,
        UNDO_LAST_MOVE
    };
}