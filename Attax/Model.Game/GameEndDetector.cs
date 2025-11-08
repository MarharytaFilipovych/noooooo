namespace Model.Game;

using Board;

public class GameEndDetector
{
    public GameEndResult CheckGameEnd(Board board, MoveValidator validator, PlayerType currentPlayer)
    {
        var (xCount, oCount) = board.CountPieces();

        if (xCount == 0)
            return new GameEndResult(true, PlayerType.O);

        if (oCount == 0)
            return new GameEndResult(true, PlayerType.X);

        if (board.IsFull())
        {
            if (xCount > oCount)
                return new GameEndResult(true, PlayerType.X);
            if (oCount > xCount)
                return new GameEndResult(true, PlayerType.O);
            return new GameEndResult(true, PlayerType.None);
        }

        var opponent = currentPlayer.GetOpponent();
        var currentCanMove = validator.GetValidMoves(currentPlayer).Count > 0;
        var opponentCanMove = validator.GetValidMoves(opponent).Count > 0;

        if (!currentCanMove && !opponentCanMove)
        {
            if (xCount > oCount)
                return new GameEndResult(true, PlayerType.X);
            if (oCount > xCount)
                return new GameEndResult(true, PlayerType.O);
            return new GameEndResult(true, PlayerType.None);
        }

        return new GameEndResult(false, PlayerType.None);
    }
}

public readonly struct GameEndResult(bool isEnded, PlayerType winner)
{
    public bool IsEnded { get; } = isEnded;
    public PlayerType Winner { get; } = winner;
}
