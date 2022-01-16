
namespace TicTacToeGame
{
    public interface IGame
    {
        PlayerTypes this[int m, int n] { get; }

        PlayerTypes[,] Board { get; }
        GameStates GameState { get; }
        PlayerTypes LastMovePlayer { get; }
        Point LastMovePoint { get; }
        int LengthM { get; }
        int LengthN { get; }
        PlayerTypes PlayerToMove { get; }
        List<Movement> MovementsAsList(int top);
    }
}