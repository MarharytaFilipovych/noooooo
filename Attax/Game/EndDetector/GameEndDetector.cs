using Model.PlayerType;
using Move.Validator;

namespace Model.Game.EndDetector;


public class GameEndDetector : IGameEndDetector
{
    public GameEndResult CheckGameEnd(Board.Board board, IMoveValidator validator, PlayerType.PlayerType currentPlayer)
    {
        var (xCount, oCount) = board.CountPieces();
        
        var eliminationResult = CheckElimination(xCount, oCount);
        if (eliminationResult.IsEnded) return eliminationResult;
        
        return board.IsFull() 
            ? DetermineWinnerByCount(xCount, oCount) 
            : CheckNoMovesAvailable(board, validator, currentPlayer, xCount, oCount);
    }
    
    private static GameEndResult CheckElimination(int xCount, int oCount)
    {
        if (xCount == 0) return new GameEndResult(true, PlayerType.PlayerType.O);
        return oCount == 0 
            ? new GameEndResult(true, PlayerType.PlayerType.X) 
            : new GameEndResult(false, PlayerType.PlayerType.None);
    }
    
    private static GameEndResult DetermineWinnerByCount(int xCount, int oCount)
    {
        if (xCount > oCount) return new GameEndResult(true, PlayerType.PlayerType.X);
        return oCount > xCount
            ? new GameEndResult(true, PlayerType.PlayerType.O) 
            : new GameEndResult(true, PlayerType.PlayerType.None); 
    }
    
    private static GameEndResult CheckNoMovesAvailable(Board.Board board, IMoveValidator validator, 
        PlayerType.PlayerType currentPlayer, int xCount, int oCount)
    {
        var opponent = currentPlayer.GetOpponent();
        var currentCanMove = validator.GetValidMoves(board, currentPlayer).Count > 0;
        var opponentCanMove = validator.GetValidMoves(board, opponent).Count > 0;
        
        if (currentCanMove || opponentCanMove)
            return new GameEndResult(false, PlayerType.PlayerType.None);
        
        return DetermineWinnerByCount(xCount, oCount);
    }
}

