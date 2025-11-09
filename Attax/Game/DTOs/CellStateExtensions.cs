namespace Model.Game.DTOs;

public static class CellStateExtensions
{
    public static char ToSymbol(this CellState cell)
    {
        if (cell.IsBlocked) return '#';
        if (cell.IsEmpty) return '.';
        return cell.OccupiedBy == PlayerType.PlayerType.X ? 'X' : 'O';
    }
}