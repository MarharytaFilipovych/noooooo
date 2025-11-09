using Model.PlayerType;

namespace Model.Board;

using System.Collections.Generic;

public class MoveExecutor(Board board)
{
    private readonly Board _board = board ?? throw new ArgumentNullException(nameof(board));
    
    
    public List<Position.Position> ExecuteMove(Move move, PlayerType.PlayerType player)
    {
        var toCell = _board.GetCell(move.To);
        toCell.OccupyBy(player);

        if (move.Type != MoveType.Jump) 
            return ConvertAdjacentPieces(move.To, player);
        
        var fromCell = _board.GetCell(move.From);
        fromCell.Clear();

        return ConvertAdjacentPieces(move.To, player);
    }

    private List<Position.Position> ConvertAdjacentPieces(Position.Position pos, PlayerType.PlayerType player)
    {
        var converted = new List<Position.Position>();
        var opponent = player.GetOpponent();

        for (var dRow = -1; dRow <= 1; dRow++)
        {
            for (var dCol = -1; dCol <= 1; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                var adjacentPos = new Position.Position(pos.Row + dRow, pos.Col + dCol);
                
                if (!_board.IsValidPosition(adjacentPos))
                    continue;

                var adjacentCell = _board.GetCell(adjacentPos);
                
                if (!adjacentCell.IsOccupied || adjacentCell.OccupiedBy != opponent) 
                    continue;
                
                adjacentCell.ConvertTo(player);
                converted.Add(adjacentPos);
            }
        }

        return converted;
    }
}