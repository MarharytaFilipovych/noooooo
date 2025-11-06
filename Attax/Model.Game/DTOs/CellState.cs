namespace Model.Game.DTOs;

using Model;

public record CellState(
    PlayerType OccupiedBy,
    bool IsBlocked
)
{
    public bool IsEmpty => OccupiedBy == PlayerType.None && !IsBlocked;
    public bool IsOccupied => OccupiedBy != PlayerType.None;
}