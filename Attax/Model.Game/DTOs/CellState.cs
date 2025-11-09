namespace Model.Game.DTOs;

using Model;

public record CellState(PlayerType.PlayerType OccupiedBy, bool IsBlocked)
{
    public bool IsEmpty => OccupiedBy == PlayerType.PlayerType.None && !IsBlocked;
    public bool IsOccupied => OccupiedBy != PlayerType.PlayerType.None;
}