namespace TicTacToeGame
{
    [Serializable]
    public enum GameStates
    {
        NEW_TURN_READY,
        THINKING,
        PAUSED,
        CHECKING_RESULT,
        X_WON,
        O_WON,
        DRAW
    };
}