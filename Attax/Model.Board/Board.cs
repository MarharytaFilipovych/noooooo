namespace Model.Board;

using System;
using System.Collections.Generic;
using Model.Core;

public class Board
{
    private readonly Cell[,] cells;
    private readonly int size;

    public int Size => size;

    public Board(int size)
    {
        if (size < 5)
        {
            throw new ArgumentException("Board size must be at least 5");
        }

        this.size = size;
        cells = new Cell[size, size];
        
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                cells[row, col] = new Cell();
            }
        }
    }

    public void Initialize(IBoardLayout layout)
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (layout.IsBlocked(row, col, size))
                {
                    cells[row, col].MarkAsBlocked();
                }
            }
        }

        cells[0, 0].OccupyBy(PlayerType.X);
        cells[0, size - 1].OccupyBy(PlayerType.O);
        cells[size - 1, 0].OccupyBy(PlayerType.O);
        cells[size - 1, size - 1].OccupyBy(PlayerType.X);
    }

    public bool IsValidPosition(Position pos)
    {
        return pos.Row >= 0 && pos.Row < size && 
               pos.Col >= 0 && pos.Col < size;
    }

    public Cell GetCell(Position pos)
    {
        if (!IsValidPosition(pos))
        {
            throw new ArgumentException($"Position {pos} is out of bounds");
        }
        return cells[pos.Row, pos.Col];
    }

    public bool IsValidMove(Move move, PlayerType player)
    {
        if (!IsValidPosition(move.From) || !IsValidPosition(move.To))
        {
            return false;
        }

        if (!move.IsValid)
        {
            return false;
        }

        var fromCell = GetCell(move.From);
        if (!fromCell.IsOccupied || fromCell.OccupiedBy != player)
        {
            return false;
        }

        var toCell = GetCell(move.To);
        if (!toCell.IsEmpty)
        {
            return false;
        }

        return true;
    }

    public List<Position> ExecuteMove(Move move, PlayerType player)
    {
        var toCell = GetCell(move.To);
        toCell.OccupyBy(player);

        if (move.Type == MoveType.Jump)
        {
            var fromCell = GetCell(move.From);
            fromCell.Clear();
        }

        var convertedPositions = ConvertAdjacentPieces(move.To, player);
        
        return convertedPositions;
    }

    private List<Position> ConvertAdjacentPieces(Position pos, PlayerType player)
    {
        var converted = new List<Position>();
        var opponent = GetOpponent(player);

        for (int dRow = -1; dRow <= 1; dRow++)
        {
            for (int dCol = -1; dCol <= 1; dCol++)
            {
                if (dRow == 0 && dCol == 0)
                    continue;

                var adjacentPos = new Position(pos.Row + dRow, pos.Col + dCol);
                
                if (!IsValidPosition(adjacentPos))
                    continue;

                var adjacentCell = GetCell(adjacentPos);
                if (adjacentCell.IsOccupied && adjacentCell.OccupiedBy == opponent)
                {
                    adjacentCell.ConvertTo(player);
                    converted.Add(adjacentPos);
                }
            }
        }

        return converted;
    }

    public List<Move> GetValidMoves(PlayerType player)
    {
        var validMoves = new List<Move>();

        var playerPositions = new List<Position>();
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                var pos = new Position(row, col);
                var cell = GetCell(pos);
                if (cell.IsOccupied && cell.OccupiedBy == player)
                {
                    playerPositions.Add(pos);
                }
            }
        }

        foreach (var from in playerPositions)
        {
            for (int dRow = -2; dRow <= 2; dRow++)
            {
                for (int dCol = -2; dCol <= 2; dCol++)
                {
                    if (dRow == 0 && dCol == 0)
                        continue;

                    var to = new Position(from.Row + dRow, from.Col + dCol);
                    var move = new Move(from, to);
                    
                    if (IsValidMove(move, player))
                    {
                        validMoves.Add(move);
                    }
                }
            }
        }

        return validMoves;
    }

    public (int xCount, int oCount) CountPieces()
    {
        int xCount = 0;
        int oCount = 0;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                var cell = cells[row, col];
                if (cell.OccupiedBy == PlayerType.X)
                    xCount++;
                else if (cell.OccupiedBy == PlayerType.O)
                    oCount++;
            }
        }

        return (xCount, oCount);
    }

    public bool IsFull()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (cells[row, col].IsEmpty)
                    return false;
            }
        }
        return true;
    }

    public Board Clone()
    {
        var clonedBoard = new Board(size);
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                clonedBoard.cells[row, col] = cells[row, col].Clone();
            }
        }
        return clonedBoard;
    }

    public Cell[,] GetCells()
    {
        var copy = new Cell[size, size];
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                copy[row, col] = cells[row, col].Clone();
            }
        }
        return copy;
    }

    private PlayerType GetOpponent(PlayerType player)
    {
        return player == PlayerType.X ? PlayerType.O : PlayerType.X;
    }
}
