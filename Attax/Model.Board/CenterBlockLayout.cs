namespace Model.Board;

public class CenterBlockLayout : IBoardLayout
{
    public string Name => "Center Block";

    public bool IsBlocked(int row, int col, int boardSize)
    {
        if (boardSize != 7)
        {
            int center = boardSize / 2;
            return (row == center && col == center);
        }
        
        if (row == 1 && col == 3) return true;
        if (row == 2 && (col == 2 || col == 4)) return true;
        if (row == 3 && (col == 1 || col == 5)) return true;
        if (row == 4 && (col == 2 || col == 4)) return true;
        if (row == 5 && col == 3) return true;
        
        return false;
    }
}
