namespace TicTacToeGame
{
    [Serializable]
    public enum GameEventes
    {
        PAUSE,
        START_THINKING,
        DO_MOVE,
        X_WON,
        O_WON,
        DRAW,
        UNDO_LAST_MOVE
    };
}