using Model.PlayerType;
using Move.Validator;

namespace Model.Game;

public class GameEndDetector 
{
    public GameEndResult CheckGameEnd(Board.Board board, IMoveValidator validator, PlayerType.PlayerType currentPlayer)
    {
        var (xCount, oCount) = board.CountPieces();

        if (xCount == 0)
            return new GameEndResult(true, PlayerType.PlayerType.O);

        if (oCount == 0)
            return new GameEndResult(true, PlayerType.PlayerType.X);

        if (board.IsFull())
        {
            if (xCount > oCount)
                return new GameEndResult(true, PlayerType.PlayerType.X);
            if (oCount > xCount)
                return new GameEndResult(true, PlayerType.PlayerType.O);
            return new GameEndResult(true, PlayerType.PlayerType.None);
        }

        var opponent = currentPlayer.GetOpponent();
        var currentCanMove = validator.GetValidMoves(board, currentPlayer).Count > 0;
        var opponentCanMove = validator.GetValidMoves(board, opponent).Count > 0;

        if (!currentCanMove && !opponentCanMove)
        {
            if (xCount > oCount)
                return new GameEndResult(true, PlayerType.PlayerType.X);
            if (oCount > xCount)
                return new GameEndResult(true, PlayerType.PlayerType.O);
            return new GameEndResult(true, PlayerType.PlayerType.None);
        }

        return new GameEndResult(false, PlayerType.PlayerType.None);
    }
}

public readonly struct GameEndResult(bool isEnded, PlayerType.PlayerType winner)
{
    public bool IsEnded { get; } = isEnded;
    public PlayerType.PlayerType Winner { get; } = winner;
}

