using Model.Board;

namespace Move.Validator;

public interface IMoveValidator
{
    List<Move> GetValidMoves(Board board,Model.PlayerType.PlayerType player);
    public bool IsValidMove(Board board, Move move, Model.PlayerType.PlayerType player, out string? error);
    
}