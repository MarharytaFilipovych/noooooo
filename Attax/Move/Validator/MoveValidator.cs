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
    
    public bool IsValidMove(Board board, Move move, Model.PlayerType.PlayerType player)
    {
        if (!board.IsValidPosition(move.From) || !board.IsValidPosition(move.To))
            return false;

        if (!move.IsValid) return false;

        var fromCell = board.GetCell(move.From);
        if (!fromCell.IsOccupied || fromCell.OccupiedBy != player) return false;

        var toCell = board.GetCell(move.To);
        return toCell.IsEmpty;
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

                if (IsValidMove(board, move, player))
                    moves.Add(move);
            }
        }

        return moves;
    }
}
