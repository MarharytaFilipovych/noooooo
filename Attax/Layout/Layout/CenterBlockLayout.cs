namespace Layout.Layout;

public class CenterBlockLayout : IBoardLayout
{
    public string Name => "Center Block";
    public LayoutType.LayoutType Type => LayoutType.LayoutType.CenterBlock;

    public bool IsBlocked(int row, int col, int boardSize)
    {
        if (boardSize != 7)
        {
            var center = boardSize / 2;
            return (row == center && col == center);
        }
        
        switch (row)
        {
            case 1 when col == 3:
            case 2 when col is 2 or 4:
            case 3 when col is 1 or 5:
            case 4 when col is 2 or 4:
            case 5 when col == 3:
                return true;
            default:
                return false;
        }
    }
}
