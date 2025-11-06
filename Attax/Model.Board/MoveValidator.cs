namespace Model.Board;

using System;
using System.Collections.Generic;

public class MoveValidator(Board board)
{
    private readonly Board _board = board ?? throw new ArgumentNullException(nameof(board));

    public bool IsValidMove(Move move, PlayerType player)
    {
        if (!_board.IsValidPosition(move.From) || !_board.IsValidPosition(move.To))
            return false;

        if (!move.IsValid) return false;

        var fromCell = _board.GetCell(move.From);
        if (!fromCell.IsOccupied || fromCell.OccupiedBy != player)
            return false;

        var toCell = _board.GetCell(move.To);
        return toCell.IsEmpty;
    }

    public List<Move> GetValidMoves(PlayerType player)
    {
        var validMoves = new List<Move>();
        var playerPositions = GetPlayerPositions(player);
        playerPositions.ForEach(from => validMoves
            .AddRange(GetValidMovesFromPosition(from, player)));

        return validMoves;
    }

    private List<Position> GetPlayerPositions(PlayerType player)
    {
        var positions = new List<Position>();

        for (var row = 0; row < _board.Size; row++)
        {
            for (var col = 0; col < _board.Size; col++)
            {
                var pos = new Position(row, col);
                var cell = _board.GetCell(pos);

                if (cell.IsOccupied && cell.OccupiedBy == player)
                    positions.Add(pos);
            }
        }

        return positions;
    }

    private List<Move> GetValidMovesFromPosition(Position from, PlayerType player)
    {
        var moves = new List<Move>();

        for (var dRow = -2; dRow <= 2; dRow++)
        {
            for (var dCol = -2; dCol <= 2; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                var to = new Position(from.Row + dRow, from.Col + dCol);
                var move = new Move(from, to);

                if (IsValidMove(move, player))
                    moves.Add(move);
            }
        }

        return moves;
    }
}
