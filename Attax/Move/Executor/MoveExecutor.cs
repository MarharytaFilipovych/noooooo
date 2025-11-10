using Model;
using Model.Board;
using Model.PlayerType;

namespace Move.Executor;

public class MoveExecutor : IMoveExecutor
{
    
    public List<Model.Position.Position> ExecuteMove(Board board, Move move, PlayerType player)
    {
        var toCell = board.GetCell(move.To);
        toCell.OccupyBy(player);

        if (move.Type != MoveType.Jump) 
            return ConvertAdjacentPieces(board, move.To, player);
        
        var fromCell = board.GetCell(move.From);
        fromCell.Clear();

        return ConvertAdjacentPieces(board, move.To, player);
    }

    private List<Model.Position.Position> ConvertAdjacentPieces(Board board, Model.Position.Position pos, PlayerType player)
    {
        var converted = new List<Model.Position.Position>();
        var opponent = player.GetOpponent();

        for (var dRow = -1; dRow <= 1; dRow++)
        {
            for (var dCol = -1; dCol <= 1; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                var adjacentPos = new Model.Position.Position(pos.Row + dRow, pos.Col + dCol);
                
                if (!board.IsValidPosition(adjacentPos))
                    continue;

                var adjacentCell = board.GetCell(adjacentPos);
                
                if (!adjacentCell.IsOccupied || adjacentCell.OccupiedBy != opponent) 
                    continue;
                
                adjacentCell.ConvertTo(player);
                converted.Add(adjacentPos);
            }
        }

        return converted;
    }
}