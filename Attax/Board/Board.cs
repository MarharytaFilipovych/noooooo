using Layout;

namespace Model.Board;

using System;

public class Board
{
    
    private readonly Cell[,] _cells;

    public int Size { get; }

    public Board(int? size = null)
    {
        var boardSize = size ?? BoardConstants.DefaultSize;
        if (boardSize is < BoardConstants.MinBoardSize or > BoardConstants.MaxBoardSize) 
            throw new ArgumentException($"Board size must be between {BoardConstants.MinBoardSize} " +
                                        $"and {BoardConstants.MaxBoardSize}");

        Size = boardSize;
        _cells = new Cell[Size, Size];

        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                _cells[row, col] = new Cell();
            }
        }
    }


    public void Initialize(IBoardLayout layout)
    {
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                if (layout.IsBlocked(row, col, Size)) 
                    _cells[row, col].MarkAsBlocked();
            }
        }

        _cells[0, 0].OccupyBy(PlayerType.PlayerType.X);
        _cells[0, Size - 1].OccupyBy(PlayerType.PlayerType.O);
        _cells[Size - 1, 0].OccupyBy(PlayerType.PlayerType.O);
        _cells[Size - 1, Size - 1].OccupyBy(PlayerType.PlayerType.X);
    }

    public bool IsValidPosition(Position.Position pos)
    {
        return pos.Row >= 0 && pos.Row < Size && 
               pos.Col >= 0 && pos.Col < Size;
    }

    public Cell GetCell(Position.Position pos) =>
        !IsValidPosition(pos) 
            ? throw new ArgumentException($"Position {pos} is out of bounds") 
            : _cells[pos.Row, pos.Col];

    public (int xCount, int oCount) CountPieces()
    {
        var xCount = 0;
        var oCount = 0;

        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                var cell = _cells[row, col];
                
                switch (cell.OccupiedBy)
                {
                    case PlayerType.PlayerType.X:
                        xCount++;
                        break;
                    case PlayerType.PlayerType.O:
                        oCount++;
                        break;
                    case PlayerType.PlayerType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return (xCount, oCount);
    }

    public bool IsFull()
    {
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                if (_cells[row, col].IsEmpty) return false;
            }
        }
        return true;
    }

    public Board Clone()
    {
        var clonedBoard = new Board(Size);
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                clonedBoard._cells[row, col] = _cells[row, col].Clone();
            }
        }
        return clonedBoard;
    }

    public Cell[,] GetCells()
    {
        var copy = new Cell[Size, Size];
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                copy[row, col] = _cells[row, col].Clone();
            }
        }
        return copy;
    }
}