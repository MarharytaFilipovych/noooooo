using Model.Board;

namespace Move.Validator;

public class MoveValidator : IMoveValidator
{

    public List<Move> GetValidMoves(Board board, Model.PlayerType.PlayerType player)
    {
        var validMoves = new List<Move>();
        var playerPositions = GetPlayerPositions(board, player);
        playerPositions.ForEach(from => validMoves
            .AddRange(GetValidMovesFromPosition(board, from, player)));

        return validMoves;
    }
    
    public bool IsValidMove(Board board, Move move, Model.PlayerType.PlayerType player, out string? error)
    {
        error = null;

        if (!move.IsValid)
        {
            error = $"Incorrect move distance: {move.From} to {move.To}. " +
                    "Moves must be a distance of 1 (Clone) or 2 (Jump).";
            return false;
        }
        
        if (!board.IsValidPosition(move.From) || !board.IsValidPosition(move.To))
        {
            error = $"One of the positions ({move.From} or {move.To}) is out of board bounds!!!!!";
            return false;
        }

        if (!board.GetCell(move.From).IsOccupied || board.GetCell(move.From).OccupiedBy != player)
        {
            error = $"The starting position {move.From} is not occupied by your piece, you dummy!";
            return false;
        }

        if (!board.GetCell(move.To).IsEmpty)
        {
            error = $"The target position {move.To} is not empty (it is occupied or blocked). Be careful!";
            return false;
        }

        return true;
    }

    
    private List<Model.Position.Position> GetPlayerPositions(Board board, Model.PlayerType.PlayerType player)
    {
        var positions = new List<Model.Position.Position>();

        for (var row = 0; row < board.Size; row++)
        {
            for (var col = 0; col < board.Size; col++)
            {
                var pos = new Model.Position.Position(row, col);
                var cell = board.GetCell(pos);

                if (cell.IsOccupied && cell.OccupiedBy == player)
                    positions.Add(pos);
            }
        }

        return positions;
    }

    private List<Move> GetValidMovesFromPosition(Board board, Model.Position.Position from, Model.PlayerType.PlayerType player)
    {
        var moves = new List<Move>();

        for (var dRow = -2; dRow <= 2; dRow++)
        {
            for (var dCol = -2; dCol <= 2; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                var to = new Model.Position.Position(from.Row + dRow, from.Col + dCol);
                var move = new Move(from, to);

                if (IsValidMove(board, move, player, out var _)) moves.Add(move);
            }
        }

        return moves;
    }
}