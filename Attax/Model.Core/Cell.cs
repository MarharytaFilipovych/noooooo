namespace Model;

public class Cell
{
    public PlayerType OccupiedBy { get; private set; } = PlayerType.None;

    public bool IsBlocked { get; private set; }

    public bool IsEmpty => OccupiedBy == PlayerType.None && !IsBlocked;
    
    public bool IsOccupied => OccupiedBy != PlayerType.None;

    public void MarkAsBlocked()
    {
        if (IsOccupied)
        {
            throw new InvalidOperationException("Cannot block an occupied cell");
        }
        IsBlocked = true;
    }

    public void OccupyBy(PlayerType player)
    {
        if (player == PlayerType.None)
        {
            throw new ArgumentException("Cannot occupy cell with None player type");
        }
        
        if (IsBlocked)
        {
            throw new InvalidOperationException("Cannot occupy a blocked cell");
        }
        
        if (IsOccupied)
        {
            throw new InvalidOperationException("Cell is already occupied");
        }

        OccupiedBy = player;
    }

    public void ConvertTo(PlayerType player)
    {
        if (player == PlayerType.None)
        {
            throw new ArgumentException("Cannot convert cell to None player type");
        }
        
        if (!IsOccupied)
        {
            throw new InvalidOperationException("Cannot convert an empty cell");
        }
        
        if (IsBlocked)
        {
            throw new InvalidOperationException("Cannot convert a blocked cell");
        }

        OccupiedBy = player;
    }

    public void Clear()
    {
        if (IsBlocked)
        {
            throw new InvalidOperationException("Cannot clear a blocked cell");
        }
        
        OccupiedBy = PlayerType.None;
    }

    public Cell Clone()
    {
        var cell = new Cell
        {
            OccupiedBy = OccupiedBy,
            IsBlocked = IsBlocked
        };
        return cell;
    }
}
